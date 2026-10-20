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
    /// <summary>Підсистема бронювання номерів у готелях.</summary>
    public class RoomBookingService : IRoomBookingService
    {
        private const string EntityName = "Бронювання номера";
        private const int CancellationDeadlineDays = 0;

        private static readonly Expression<Func<RoomBooking, object>>[] Details = { b => b.Room.Hotel, b => b.User };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public RoomBookingService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<RoomBooking> Bookings => _unitOfWork.Repository<RoomBooking>();

        public RoomBookingModel Book(int userId, BookRoomModel model)
        {
            User user = UserGuard.GetRegistered(_unitOfWork, userId);
            HotelRoom room = _unitOfWork.Repository<HotelRoom>().GetRequired(model.RoomId, "Номер");
            int nights = StayPeriod.Validate(model.CheckIn, model.CheckOut, _clock.Today);
            Guard.InRange(model.Guests, 1, room.Capacity, "Кількість гостей");

            DateTime checkIn = model.CheckIn.Date;
            DateTime checkOut = model.CheckOut.Date;
            // періоди [заїзд; виїзд) перетинаються, якщо кожен починається раніше, ніж закінчується інший
            if (Bookings.Any(b => b.RoomId == room.Id
                                  && b.Status == BookingStatus.Active
                                  && b.CheckIn < checkOut
                                  && checkIn < b.CheckOut))
            {
                throw new BusinessConflictException("Номер уже заброньовано на ці дати.");
            }

            var booking = new RoomBooking
            {
                RoomId = room.Id,
                UserId = user.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Guests = model.Guests,
                TotalPrice = room.PricePerNight * nights,
                Status = BookingStatus.Active,
                CreatedAt = _clock.Now
            };
            Bookings.Add(booking);
            _unitOfWork.SaveChanges();
            return Bookings.GetRequired(booking.Id, EntityName, Details).ToModel();
        }

        public IReadOnlyList<RoomBookingModel> GetByUser(int userId)
        {
            return ToSortedModels(Bookings.Find(b => b.UserId == userId, Details));
        }

        public IReadOnlyList<RoomBookingModel> GetAll()
        {
            return ToSortedModels(Bookings.GetAll(Details));
        }

        public void Cancel(int bookingId, int userId)
        {
            User user = UserGuard.GetRegistered(_unitOfWork, userId);
            RoomBooking booking = Bookings.GetRequired(bookingId, EntityName, Details);
            BookingCancellation.Cancel(_unitOfWork, booking, user, booking.CheckIn, _clock.Today, CancellationDeadlineDays);
        }

        private static IReadOnlyList<RoomBookingModel> ToSortedModels(IEnumerable<RoomBooking> bookings)
        {
            return bookings.OrderByDescending(b => b.CreatedAt).Select(b => b.ToModel()).ToList();
        }
    }
}
