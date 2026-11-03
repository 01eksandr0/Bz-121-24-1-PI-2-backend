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
    /// <summary>Підсистема бронювання номерів у готелях.</summary>
    internal class RoomBookingService : BookingServiceBase<RoomBooking, RoomBookingModel>, IRoomBookingService
    {
        private static readonly Expression<Func<RoomBooking, object>>[] Includes = { b => b.Room.Hotel, b => b.User };

        public RoomBookingService(IUnitOfWork unitOfWork, IClock clock) : base(unitOfWork, clock)
        {
        }

        protected override string EntityName => "Бронювання номера";

        protected override Expression<Func<RoomBooking, object>>[] Details => Includes;

        protected override int CancellationDeadlineDays => BusinessRules.RoomCancellationDeadlineDays;

        public RoomBookingModel Book(int userId, RoomBookingInputModel model)
        {
            User user = UserGuard.GetRegistered(UnitOfWork, userId);
            Room room = UnitOfWork.Repository<Room>().GetRequired(model.RoomId, "Номер");
            int nights = StayPeriod.Validate(model.CheckIn, model.CheckOut, Clock.Today);
            Guard.InRange(model.Guests, BusinessRules.MinQuantity, room.Capacity, "Кількість гостей");

            RoomBooking booking = UnitOfWork.InTransaction(() =>
            {
                int busy = Occupancy.Rooms(UnitOfWork, new[] { room.Id }, model.CheckIn, model.CheckOut)[room.Id];
                if (busy >= room.Quantity)
                {
                    throw new ConflictException("Усі номери цієї категорії на ці дати вже заброньовано.");
                }
                return Add(new RoomBooking
                {
                    RoomId = room.Id,
                    UserId = user.Id,
                    CheckIn = model.CheckIn.Date,
                    CheckOut = model.CheckOut.Date,
                    Guests = model.Guests,
                    TotalPrice = room.PricePerNight * nights
                });
            });
            return Reload(booking);
        }

        protected override DateTime StartOf(RoomBooking booking) => booking.CheckIn;

        protected override RoomBookingModel ToModel(RoomBooking booking) => booking.ToModel();
    }
}
