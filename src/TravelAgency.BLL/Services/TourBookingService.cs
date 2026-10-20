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
    /// <summary>Підсистема бронювання турів.</summary>
    public class TourBookingService : ITourBookingService
    {
        private const string EntityName = "Бронювання туру";

        private static readonly Expression<Func<TourBooking, object>>[] Details = { b => b.Tour, b => b.User };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public TourBookingService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<TourBooking> Bookings => _unitOfWork.Repository<TourBooking>();

        public TourBookingModel Book(int userId, BookTourModel model)
        {
            User user = UserGuard.GetRegistered(_unitOfWork, userId);
            Guard.InRange(model.Persons, 1, BusinessRules.MaxPersonsPerBooking, "Кількість осіб");
            Tour tour = _unitOfWork.Repository<Tour>().GetRequired(model.TourId, "Тур");

            if (tour.StartDate.Date <= _clock.Today)
            {
                throw new BusinessConflictException("Тур уже почався або завершився.");
            }
            if (Bookings.Any(b => b.TourId == tour.Id && b.UserId == userId && b.Status == BookingStatus.Active))
            {
                throw new BusinessConflictException("У вас уже є активне бронювання цього туру.");
            }
            int booked = Bookings.Find(b => b.TourId == tour.Id && b.Status == BookingStatus.Active).Sum(b => b.Persons);
            int free = tour.Capacity - booked;
            if (model.Persons > free)
            {
                throw new BusinessConflictException("Недостатньо вільних місць. Залишилось: " + free + ".");
            }

            var booking = new TourBooking
            {
                TourId = tour.Id,
                UserId = user.Id,
                Persons = model.Persons,
                TotalPrice = Pricing.ApplyDiscount(tour.PricePerPerson, tour.DiscountPercent) * model.Persons,
                Status = BookingStatus.Active,
                CreatedAt = _clock.Now
            };
            Bookings.Add(booking);
            _unitOfWork.SaveChanges();
            return Bookings.GetRequired(booking.Id, EntityName, Details).ToModel();
        }

        public IReadOnlyList<TourBookingModel> GetByUser(int userId)
        {
            return ToSortedModels(Bookings.Find(b => b.UserId == userId, Details));
        }

        public IReadOnlyList<TourBookingModel> GetAll()
        {
            return ToSortedModels(Bookings.GetAll(Details));
        }

        public void Cancel(int bookingId, int userId)
        {
            User user = UserGuard.GetRegistered(_unitOfWork, userId);
            TourBooking booking = Bookings.GetRequired(bookingId, EntityName, Details);
            BookingCancellation.Cancel(_unitOfWork, booking, user, booking.Tour.StartDate.Date, _clock.Today,
                BusinessRules.TourCancellationDeadlineDays);
        }

        private static IReadOnlyList<TourBookingModel> ToSortedModels(IEnumerable<TourBooking> bookings)
        {
            return bookings.OrderByDescending(b => b.CreatedAt).Select(b => b.ToModel()).ToList();
        }
    }
}
