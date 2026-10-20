using System.Collections.Generic;
using TravelAgency.BLL.Models.Bookings;

namespace TravelAgency.BLL.Interfaces
{
    public interface ITourBookingService
    {
        TourBookingModel Book(int userId, BookTourModel model);

        IReadOnlyList<TourBookingModel> GetByUser(int userId);

        IReadOnlyList<TourBookingModel> GetAll();

        void Cancel(int bookingId, int userId);
    }
}
