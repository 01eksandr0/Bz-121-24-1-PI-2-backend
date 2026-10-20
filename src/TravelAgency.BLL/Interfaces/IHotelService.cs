using System;
using System.Collections.Generic;
using TravelAgency.BLL.Models.Hotels;

namespace TravelAgency.BLL.Interfaces
{
    public interface IHotelService
    {
        IReadOnlyList<HotelModel> Search(HotelFilterModel filter);

        HotelModel GetById(int id);

        IReadOnlyList<HotelRoomModel> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut, int guests);

        HotelModel Create(HotelInputModel input);

        HotelRoomModel AddRoom(int hotelId, HotelRoomInputModel input);
    }
}
