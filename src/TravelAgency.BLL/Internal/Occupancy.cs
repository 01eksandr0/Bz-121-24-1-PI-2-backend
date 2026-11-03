using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Internal
{
    /// <summary>
    /// Скільки вже зайнято активними бронюваннями (одне місце для правила - і для показу, і для бронювання).
    /// Кожен метод - один запит до БД на весь список id; для id без бронювань повертається 0.
    /// </summary>
    internal static class Occupancy
    {
        /// <summary>Зайняті місця в турах: сума осіб.</summary>
        public static Dictionary<int, int> TourPlaces(IUnitOfWork unitOfWork, IReadOnlyCollection<int> tourIds)
        {
            List<int> ids = tourIds.Distinct().ToList();
            ILookup<int, TourBooking> bookings = unitOfWork.Repository<TourBooking>()
                .FindAll(b => ids.Contains(b.TourId) && b.Status == BookingStatus.Active)
                .ToLookup(b => b.TourId);
            return ids.ToDictionary(id => id, id => bookings[id].Sum(b => b.Persons));
        }

        /// <summary>Зайняті місця на рейсах: сума квитків.</summary>
        public static Dictionary<int, int> TripPlaces(IUnitOfWork unitOfWork, IReadOnlyCollection<int> tripIds)
        {
            List<int> ids = tripIds.Distinct().ToList();
            ILookup<int, TicketBooking> bookings = unitOfWork.Repository<TicketBooking>()
                .FindAll(b => ids.Contains(b.TripId) && b.Status == BookingStatus.Active)
                .ToLookup(b => b.TripId);
            return ids.ToDictionary(id => id, id => bookings[id].Sum(b => b.Seats));
        }

        /// <summary>
        /// Зайняті номери кожної категорії на період [заїзд; виїзд). Періоди перетинаються,
        /// якщо кожен починається раніше, ніж закінчується інший (виїзд і заїзд в один день - не перетин).
        /// </summary>
        public static Dictionary<int, int> Rooms(IUnitOfWork unitOfWork, IReadOnlyCollection<int> roomIds,
            DateTime checkIn, DateTime checkOut)
        {
            List<int> ids = roomIds.Distinct().ToList();
            DateTime from = checkIn.Date;
            DateTime to = checkOut.Date;
            ILookup<int, RoomBooking> bookings = unitOfWork.Repository<RoomBooking>()
                .FindAll(b => ids.Contains(b.RoomId)
                              && b.Status == BookingStatus.Active
                              && b.CheckIn < to
                              && from < b.CheckOut)
                .ToLookup(b => b.RoomId);
            return ids.ToDictionary(id => id, id => bookings[id].Count());
        }
    }
}
