using System.Collections.Generic;
using TravelAgency.BLL.Models.Bookings;

namespace TravelAgency.BLL.Interfaces
{
    public interface ITicketBookingService
    {
        TicketBookingModel Book(int userId, BookTicketModel model);

        IReadOnlyList<TicketBookingModel> GetByUser(int userId);

        IReadOnlyList<TicketBookingModel> GetAll();

        void Cancel(int bookingId, int userId);
    }
}
