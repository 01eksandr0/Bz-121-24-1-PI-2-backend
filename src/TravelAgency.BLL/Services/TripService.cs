using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models.Trips;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>Рейси транспорту: пошук майбутніх рейсів і додавання (менеджер).</summary>
    internal class TripService : ITripService
    {
        private const string EntityName = "Рейс";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public TripService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<Trip> Trips => _unitOfWork.Repository<Trip>();

        public IReadOnlyList<TripModel> Search(TripSearchModel filter)
        {
            DateTime now = _clock.Now;
            bool anyType = !filter.Type.HasValue;
            TransportType type = filter.Type.HasValue ? filter.Type.Value.ToEntity() : default(TransportType);
            bool anyDate = !filter.Date.HasValue;
            DateTime dayStart = filter.Date.HasValue ? filter.Date.Value.Date : DateTime.MinValue;
            DateTime dayEnd = dayStart.AddDays(1);

            IEnumerable<Trip> trips = Trips.FindAll(t =>
                t.DepartureAt > now
                && (anyType || t.Type == type)
                && (anyDate || (t.DepartureAt >= dayStart && t.DepartureAt < dayEnd)));

            trips = trips.Where(t => MatchesCity(t.FromCity, filter.FromCity) && MatchesCity(t.ToCity, filter.ToCity));
            List<Trip> list = trips.OrderBy(t => t.DepartureAt).ToList();
            Dictionary<int, int> booked = Occupancy.TripPlaces(_unitOfWork, list.Select(t => t.Id).ToList());
            return list.Select(t => t.ToModel(booked[t.Id])).ToList();
        }

        public TripModel GetById(int id)
        {
            Trip trip = Trips.GetRequired(id, EntityName);
            return trip.ToModel(Occupancy.TripPlaces(_unitOfWork, new[] { id })[id]);
        }

        public TripModel Create(TripInputModel input)
        {
            if (!Enum.IsDefined(typeof(Models.TransportType), input.Type))
            {
                throw new ValidationFailedException("Невідомий вид транспорту.");
            }
            string from = Guard.RequiredText(input.FromCity, "Звідки", BusinessRules.MaxNameLength);
            string to = Guard.RequiredText(input.ToCity, "Куди", BusinessRules.MaxNameLength);
            if (string.Equals(from, to, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ValidationFailedException("Місто відправлення і прибуття мають відрізнятися.");
            }
            if (input.DepartureAt <= _clock.Now)
            {
                throw new ValidationFailedException("Час відправлення має бути в майбутньому.");
            }
            if (input.ArrivalAt <= input.DepartureAt)
            {
                throw new ValidationFailedException("Час прибуття має бути пізніше за час відправлення.");
            }
            Guard.Positive(input.Price, "Ціна квитка");
            Guard.InRange(input.Capacity, BusinessRules.MinQuantity, BusinessRules.MaxTripCapacity, "Кількість місць");

            var trip = new Trip
            {
                Type = input.Type.ToEntity(),
                FromCity = from,
                ToCity = to,
                DepartureAt = input.DepartureAt,
                ArrivalAt = input.ArrivalAt,
                Price = input.Price,
                Capacity = input.Capacity
            };
            Trips.Add(trip);
            _unitOfWork.SaveChanges();
            return trip.ToModel(0);
        }

        private static bool MatchesCity(string city, string filter)
        {
            string text = (filter ?? string.Empty).Trim();
            return text.Length == 0 || city.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
