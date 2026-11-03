using System;
using System.Collections.Generic;
using TravelAgency.BLL.Models.Hotels;

namespace TravelAgency.BLL.Interfaces
{
    public interface IHotelService
    {
        IReadOnlyList<HotelModel> Search(HotelSearchModel filter);

        HotelModel GetById(int id);

        IReadOnlyList<RoomModel> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut, int guests);

        HotelModel Create(HotelInputModel input);

        RoomModel AddRoom(int hotelId, RoomInputModel input);
    }
}
