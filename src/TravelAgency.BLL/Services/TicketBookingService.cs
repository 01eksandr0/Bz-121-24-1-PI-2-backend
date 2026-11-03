using System;
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
    internal class TicketBookingService : BookingServiceBase<TicketBooking, TicketBookingModel>, ITicketBookingService
    {
        private static readonly Expression<Func<TicketBooking, object>>[] Includes = { b => b.Trip, b => b.User };

        public TicketBookingService(IUnitOfWork unitOfWork, IClock clock) : base(unitOfWork, clock)
        {
        }

        protected override string EntityName => "Бронювання квитка";

        protected override Expression<Func<TicketBooking, object>>[] Details => Includes;

        protected override int CancellationDeadlineDays => BusinessRules.TicketCancellationDeadlineDays;

        /// <summary>Рейс має точний час відправлення, тому порівнюємо з поточним часом, а не з датою.</summary>
        protected override DateTime CancellationNow => Clock.Now;

        public TicketBookingModel Book(int userId, TicketBookingInputModel model)
        {
            User user = UserGuard.GetRegistered(UnitOfWork, userId);
            Trip trip = UnitOfWork.Repository<Trip>().GetRequired(model.TripId, "Рейс");
            Guard.InRange(model.Seats, BusinessRules.MinQuantity, BusinessRules.MaxSeatsPerBooking, "Кількість місць");

            if (trip.DepartureAt <= Clock.Now)
            {
                throw new ConflictException("Рейс уже відправився.");
            }

            TicketBooking booking = UnitOfWork.InTransaction(() =>
            {
                int free = trip.Capacity - Occupancy.TripPlaces(UnitOfWork, new[] { trip.Id })[trip.Id];
                if (model.Seats > free)
                {
                    throw new ConflictException("Недостатньо вільних місць. Залишилось: " + free + ".");
                }
                return Add(new TicketBooking
                {
                    TripId = trip.Id,
                    UserId = user.Id,
                    Seats = model.Seats,
                    TotalPrice = trip.Price * model.Seats
                });
            });
            return Reload(booking);
        }

        protected override DateTime StartOf(TicketBooking booking) => booking.Trip.DepartureAt;

        protected override TicketBookingModel ToModel(TicketBooking booking) => booking.ToModel();
    }
}
