using System.Collections.Generic;
using TravelAgency.BLL.Models.Bookings;

namespace TravelAgency.BLL.Interfaces
{
    public interface IRoomBookingService
    {
        RoomBookingModel Book(int userId, BookRoomModel model);

        IReadOnlyList<RoomBookingModel> GetByUser(int userId);

        IReadOnlyList<RoomBookingModel> GetAll();

        void Cancel(int bookingId, int userId);
    }
}
