using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>Підсистема бронювання квитків на транспорт.</summary>
    public class TicketBookingService : ITicketBookingService
    {
        private const string EntityName = "Бронювання квитка";
        private const int CancellationDeadlineDays = 0;

        private static readonly Expression<Func<TicketBooking, object>>[] Details = { b => b.Trip, b => b.User };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public TicketBookingService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<TicketBooking> Bookings => _unitOfWork.Repository<TicketBooking>();

        public TicketBookingModel Book(int userId, BookTicketModel model)
        {
            User user = UserGuard.GetRegistered(_unitOfWork, userId);
            Guard.InRange(model.Seats, 1, BusinessRules.MaxSeatsPerBooking, "Кількість місць");
            TransportTrip trip = _unitOfWork.Repository<TransportTrip>().GetRequired(model.TripId, "Рейс");

            if (trip.DepartureAt <= _clock.Now)
            {
                throw new BusinessConflictException("Рейс уже відправився.");
            }
            int booked = Bookings.Find(b => b.TripId == trip.Id && b.Status == BookingStatus.Active).Sum(b => b.Seats);
            int free = trip.SeatsTotal - booked;
            if (model.Seats > free)
            {
                throw new BusinessConflictException("Недостатньо вільних місць. Залишилось: " + free + ".");
            }

            var booking = new TicketBooking
            {
                TripId = trip.Id,
                UserId = user.Id,
                Seats = model.Seats,
                TotalPrice = trip.Price * model.Seats,
                Status = BookingStatus.Active,
                CreatedAt = _clock.Now
            };
            Bookings.Add(booking);
            _unitOfWork.SaveChanges();
            return Bookings.GetRequired(booking.Id, EntityName, Details).ToModel();
        }

        public IReadOnlyList<TicketBookingModel> GetByUser(int userId)
        {
            return ToSortedModels(Bookings.Find(b => b.UserId == userId, Details));
        }

        public IReadOnlyList<TicketBookingModel> GetAll()
        {
            return ToSortedModels(Bookings.GetAll(Details));
        }

        public void Cancel(int bookingId, int userId)
        {
            User user = UserGuard.GetRegistered(_unitOfWork, userId);
            TicketBooking booking = Bookings.GetRequired(bookingId, EntityName, Details);
            BookingCancellation.Cancel(_unitOfWork, booking, user, booking.Trip.DepartureAt, _clock.Now, CancellationDeadlineDays);
        }

        private static IReadOnlyList<TicketBookingModel> ToSortedModels(IEnumerable<TicketBooking> bookings)
        {
            return bookings.OrderByDescending(b => b.CreatedAt).Select(b => b.ToModel()).ToList();
        }
    }
}
