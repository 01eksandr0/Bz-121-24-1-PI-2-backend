using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;
using TravelAgency.BLL.Interfaces;
using TravelAgency.BLL.Internal;
using TravelAgency.BLL.Mapping;
using TravelAgency.BLL.Models.Hotels;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>Готелі та номери: пошук, вільні номери на дати, додавання (менеджер).</summary>
    public class HotelService : IHotelService
    {
        private const string EntityName = "Готель";

        private static readonly Expression<Func<Hotel, object>>[] Details = { h => h.Region.Country, h => h.Rooms };

        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public HotelService(IUnitOfWork unitOfWork, IClock clock)
        {
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        private IRepository<Hotel> Hotels => _unitOfWork.Repository<Hotel>();

        public IReadOnlyList<HotelModel> Search(HotelFilterModel filter)
        {
            bool anyCountry = !filter.CountryId.HasValue;
            int countryId = filter.CountryId ?? 0;
            bool anyRegion = !filter.RegionId.HasValue;
            int regionId = filter.RegionId ?? 0;
            int minStars = filter.MinStars ?? BusinessRules.MinStars;

            IEnumerable<HotelModel> hotels = Hotels.Find(h =>
                        (anyCountry || h.Region.CountryId == countryId)
                        && (anyRegion || h.RegionId == regionId)
                        && h.Stars >= minStars,
                    Details)
                .Select(h => h.ToModel());

            string text = (filter.Text ?? string.Empty).Trim();
            if (text.Length > 0)
            {
                hotels = hotels.Where(h => h.Name.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0
                                           || h.RegionName.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0);
            }
            return hotels.OrderByDescending(h => h.Stars).ThenBy(h => h.Name).ToList();
        }

        public HotelModel GetById(int id)
        {
            return Hotels.GetRequired(id, EntityName, Details).ToModel();
        }

        public IReadOnlyList<HotelRoomModel> GetAvailableRooms(int hotelId, DateTime checkIn, DateTime checkOut, int guests)
        {
            Hotel hotel = Hotels.GetRequired(hotelId, EntityName, h => h.Rooms);
            StayPeriod.Validate(checkIn, checkOut, _clock.Today);

            List<int> roomIds = hotel.Rooms.Select(r => r.Id).ToList();
            HashSet<int> busyRoomIds = new HashSet<int>(_unitOfWork.Repository<RoomBooking>()
                .Find(b => roomIds.Contains(b.RoomId)
                           && b.Status == BookingStatus.Active
                           && b.CheckIn < checkOut.Date
                           && checkIn.Date < b.CheckOut)
                .Select(b => b.RoomId));

            return hotel.Rooms
                .Where(r => r.Capacity >= guests && !busyRoomIds.Contains(r.Id))
                .OrderBy(r => r.PricePerNight)
                .Select(r => r.ToModel())
                .ToList();
        }

        public HotelModel Create(HotelInputModel input)
        {
            string name = Guard.RequiredText(input.Name, "Назва готелю", BusinessRules.MaxNameLength);
            Guard.InRange(input.Stars, BusinessRules.MinStars, BusinessRules.MaxStars, "Зірки");
            _unitOfWork.Repository<Region>().GetRequired(input.RegionId, "Регіон");

            var hotel = new Hotel
            {
                Name = name,
                Description = (input.Description ?? string.Empty).Trim(),
                Stars = input.Stars,
                RegionId = input.RegionId
            };
            Hotels.Add(hotel);
            _unitOfWork.SaveChanges();
            return GetById(hotel.Id);
        }

        public HotelRoomModel AddRoom(int hotelId, HotelRoomInputModel input)
        {
            Hotels.GetRequired(hotelId, EntityName);
            string name = Guard.RequiredText(input.Name, "Назва номера", BusinessRules.MaxNameLength);
            Guard.InRange(input.Capacity, 1, BusinessRules.MaxRoomCapacity, "Місткість номера");
            Guard.Positive(input.PricePerNight, "Ціна за ніч");

            var room = new HotelRoom { HotelId = hotelId, Name = name, Capacity = input.Capacity, PricePerNight = input.PricePerNight };
            _unitOfWork.Repository<HotelRoom>().Add(room);
            _unitOfWork.SaveChanges();
            return room.ToModel();
        }
    }
}
