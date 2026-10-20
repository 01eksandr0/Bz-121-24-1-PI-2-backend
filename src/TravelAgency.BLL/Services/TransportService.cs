using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Transport;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>Рейси транспорту: пошук майбутніх рейсів і додавання (менеджер).</summary>
    public class TransportService : ITransportService
    {
        private const string EntityName = "Рейс";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public TransportService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<TransportTrip> Trips => _unitOfWork.Repository<TransportTrip>();

        public IReadOnlyList<TripModel> Search(TripFilterModel filter)
        {
            DateTime now = _clock.Now;
            bool anyType = !filter.Type.HasValue;
            TransportType type = filter.Type.HasValue ? filter.Type.Value.ToEntity() : default(TransportType);
            bool anyDate = !filter.Date.HasValue;
            DateTime dayStart = filter.Date.HasValue ? filter.Date.Value.Date : DateTime.MinValue;
            DateTime dayEnd = dayStart.AddDays(1);

            IEnumerable<TransportTrip> trips = Trips.Find(t =>
                t.DepartureAt > now
                && (anyType || t.Type == type)
                && (anyDate || (t.DepartureAt >= dayStart && t.DepartureAt < dayEnd)));

            trips = trips.Where(t => MatchesCity(t.FromCity, filter.FromCity) && MatchesCity(t.ToCity, filter.ToCity));
            List<TransportTrip> list = trips.OrderBy(t => t.DepartureAt).ToList();
            Dictionary<int, int> booked = BookedSeats(list.Select(t => t.Id).ToList());
            return list.Select(t => t.ToModel(booked[t.Id])).ToList();
        }

        public TripModel GetById(int id)
        {
            TransportTrip trip = Trips.GetRequired(id, EntityName);
            return trip.ToModel(BookedSeats(new List<int> { id })[id]);
        }

        public TripModel Create(TripInputModel input)
        {
            if (!Enum.IsDefined(typeof(TransportKind), input.Type))
            {
                throw new BusinessValidationException("Невідомий вид транспорту.");
            }
            string from = Guard.RequiredText(input.FromCity, "Звідки", BusinessRules.MaxNameLength);
            string to = Guard.RequiredText(input.ToCity, "Куди", BusinessRules.MaxNameLength);
            if (string.Equals(from, to, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new BusinessValidationException("Місто відправлення і прибуття мають відрізнятися.");
            }
            if (input.DepartureAt <= _clock.Now)
            {
                throw new BusinessValidationException("Час відправлення має бути в майбутньому.");
            }
            if (input.ArrivalAt <= input.DepartureAt)
            {
                throw new BusinessValidationException("Час прибуття має бути пізніше за час відправлення.");
            }
            Guard.Positive(input.Price, "Ціна квитка");
            Guard.InRange(input.SeatsTotal, 1, BusinessRules.MaxTourCapacity, "Кількість місць");

            var trip = new TransportTrip
            {
                Type = input.Type.ToEntity(),
                FromCity = from,
                ToCity = to,
                DepartureAt = input.DepartureAt,
                ArrivalAt = input.ArrivalAt,
                Price = input.Price,
                SeatsTotal = input.SeatsTotal
            };
            Trips.Add(trip);
            _unitOfWork.SaveChanges();
            return trip.ToModel(0);
        }

        private Dictionary<int, int> BookedSeats(List<int> tripIds)
        {
            ILookup<int, TicketBooking> bookings = _unitOfWork.Repository<TicketBooking>()
                .Find(b => tripIds.Contains(b.TripId) && b.Status == BookingStatus.Active)
                .ToLookup(b => b.TripId);
            return tripIds.Distinct().ToDictionary(id => id, id => bookings[id].Sum(b => b.Seats));
        }

        private static bool MatchesCity(string city, string filter)
        {
            string text = (filter ?? string.Empty).Trim();
            return text.Length == 0 || city.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
