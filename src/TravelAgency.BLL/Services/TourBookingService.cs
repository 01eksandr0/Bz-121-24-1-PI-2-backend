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
    /// <summary>Підсистема бронювання турів.</summary>
    internal class TourBookingService : BookingServiceBase<TourBooking, TourBookingModel>, ITourBookingService
    {
        private static readonly Expression<Func<TourBooking, object>>[] Includes = { b => b.Tour, b => b.User };

        public TourBookingService(IUnitOfWork unitOfWork, IClock clock) : base(unitOfWork, clock)
        {
        }

        protected override string EntityName => "Бронювання туру";

        protected override Expression<Func<TourBooking, object>>[] Details => Includes;

        protected override int CancellationDeadlineDays => BusinessRules.TourCancellationDeadlineDays;

        public TourBookingModel Book(int userId, TourBookingInputModel model)
        {
            User user = UserGuard.GetRegistered(UnitOfWork, userId);
            Guard.InRange(model.Persons, BusinessRules.MinQuantity, BusinessRules.MaxPersonsPerBooking, "Кількість осіб");
            Tour tour = UnitOfWork.Repository<Tour>().GetRequired(model.TourId, "Тур");

            if (tour.StartDate.Date <= Clock.Today)
            {
                throw new ConflictException("Тур уже почався або завершився.");
            }

            TourBooking booking = UnitOfWork.InTransaction(() =>
            {
                if (Bookings.Any(b => b.TourId == tour.Id && b.UserId == user.Id && b.Status == BookingStatus.Active))
                {
                    throw new ConflictException("У вас уже є активне бронювання цього туру.");
                }
                int free = tour.Capacity - Occupancy.TourPlaces(UnitOfWork, new[] { tour.Id })[tour.Id];
                if (model.Persons > free)
                {
                    throw new ConflictException("Недостатньо вільних місць. Залишилось: " + free + ".");
                }
                return Add(new TourBooking
                {
                    TourId = tour.Id,
                    UserId = user.Id,
                    Persons = model.Persons,
                    TotalPrice = Pricing.ApplyDiscount(tour.PricePerPerson, tour.DiscountPercent) * model.Persons
                });
            });
            return Reload(booking);
        }

        protected override DateTime StartOf(TourBooking booking) => booking.Tour.StartDate.Date;

        protected override TourBookingModel ToModel(TourBooking booking) => booking.ToModel();
    }
}
