using System.Collections.Generic;
using System.Linq;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Models.Geography;
using TravelAgency.BLL.Models.Hotels;
using TravelAgency.BLL.Models.Tours;
using TravelAgency.BLL.Models.Transport;
using TravelAgency.BLL.Models.Users;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.BLL.Mapping
{
    /// <summary>
    /// Mapping сутностей DAL у моделі BLL. Сутності DAL не виходять за межі BLL.
    /// Методи, що читають навігаційні властивості, потребують відповідних includes.
    /// </summary>
    internal static class EntityMapper
    {
        public static UserModel ToModel(this User user)
        {
            return new UserModel { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role.ToModel() };
        }

        /// <summary>Потребує Region.Country.</summary>
        public static RegionModel ToModel(this Region region)
        {
            return new RegionModel { Id = region.Id, Name = region.Name, CountryId = region.CountryId, CountryName = region.Country.Name };
        }

        public static CountryModel ToModel(this Country country, IEnumerable<Region> regions)
        {
            return new CountryModel
            {
                Id = country.Id,
                Name = country.Name,
                Regions = regions.OrderBy(r => r.Name)
                    .Select(r => new RegionModel { Id = r.Id, Name = r.Name, CountryId = country.Id, CountryName = country.Name })
                    .ToList()
            };
        }

        /// <summary>Потребує Region.Country. bookedPlaces - сума місць в активних бронюваннях.</summary>
        public static TourModel ToModel(this Tour tour, int bookedPlaces)
        {
            return new TourModel
            {
                Id = tour.Id,
                Title = tour.Title,
                Description = tour.Description,
                Type = tour.Type.ToModel(),
                RegionId = tour.RegionId,
                RegionName = tour.Region.Name,
                CountryId = tour.Region.CountryId,
                CountryName = tour.Region.Country.Name,
                StartDate = tour.StartDate,
                EndDate = tour.EndDate,
                Nights = (tour.EndDate.Date - tour.StartDate.Date).Days,
                PricePerPerson = tour.PricePerPerson,
                DiscountPercent = tour.DiscountPercent,
                FinalPrice = Pricing.ApplyDiscount(tour.PricePerPerson, tour.DiscountPercent),
                IsHot = tour.DiscountPercent > 0,
                Capacity = tour.Capacity,
                FreePlaces = tour.Capacity - bookedPlaces
            };
        }

        public static void ApplyTo(this TourInputModel input, Tour tour)
        {
            tour.Title = input.Title.Trim();
            tour.Description = (input.Description ?? string.Empty).Trim();
            tour.Type = input.Type.ToEntity();
            tour.RegionId = input.RegionId;
            tour.StartDate = input.StartDate.Date;
            tour.EndDate = input.EndDate.Date;
            tour.PricePerPerson = input.PricePerPerson;
            tour.DiscountPercent = input.DiscountPercent;
            tour.Capacity = input.Capacity;
        }

        /// <summary>Потребує Tour і User.</summary>
        public static TourBookingModel ToModel(this TourBooking booking)
        {
            return new TourBookingModel
            {
                Id = booking.Id,
                TourId = booking.TourId,
                TourTitle = booking.Tour.Title,
                TourStartDate = booking.Tour.StartDate,
                UserId = booking.UserId,
                UserName = booking.User.FullName,
                Persons = booking.Persons,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToModel(),
                CreatedAt = booking.CreatedAt
            };
        }

        public static HotelRoomModel ToModel(this HotelRoom room)
        {
            return new HotelRoomModel { Id = room.Id, HotelId = room.HotelId, Name = room.Name, Capacity = room.Capacity, PricePerNight = room.PricePerNight };
        }

        /// <summary>Потребує Region.Country і Rooms.</summary>
        public static HotelModel ToModel(this Hotel hotel)
        {
            List<HotelRoomModel> rooms = hotel.Rooms.OrderBy(r => r.PricePerNight).Select(r => r.ToModel()).ToList();
            return new HotelModel
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Description = hotel.Description,
                Stars = hotel.Stars,
                RegionId = hotel.RegionId,
                RegionName = hotel.Region.Name,
                CountryName = hotel.Region.Country.Name,
                MinPricePerNight = rooms.Count == 0 ? 0 : rooms.Min(r => r.PricePerNight),
                Rooms = rooms
            };
        }

        /// <summary>Потребує Room.Hotel і User.</summary>
        public static RoomBookingModel ToModel(this RoomBooking booking)
        {
            return new RoomBookingModel
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                RoomName = booking.Room.Name,
                HotelName = booking.Room.Hotel.Name,
                UserId = booking.UserId,
                UserName = booking.User.FullName,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Nights = (booking.CheckOut.Date - booking.CheckIn.Date).Days,
                Guests = booking.Guests,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToModel(),
                CreatedAt = booking.CreatedAt
            };
        }

        public static TripModel ToModel(this TransportTrip trip, int bookedSeats)
        {
            return new TripModel
            {
                Id = trip.Id,
                Type = trip.Type.ToModel(),
                FromCity = trip.FromCity,
                ToCity = trip.ToCity,
                DepartureAt = trip.DepartureAt,
                ArrivalAt = trip.ArrivalAt,
                Price = trip.Price,
                SeatsTotal = trip.SeatsTotal,
                FreeSeats = trip.SeatsTotal - bookedSeats
            };
        }

        /// <summary>Потребує Trip і User.</summary>
        public static TicketBookingModel ToModel(this TicketBooking booking)
        {
            return new TicketBookingModel
            {
                Id = booking.Id,
                TripId = booking.TripId,
                Route = booking.Trip.FromCity + " - " + booking.Trip.ToCity,
                TransportType = booking.Trip.Type.ToModel(),
                DepartureAt = booking.Trip.DepartureAt,
                UserId = booking.UserId,
                UserName = booking.User.FullName,
                Seats = booking.Seats,
                TotalPrice = booking.TotalPrice,
                Status = booking.Status.ToModel(),
                CreatedAt = booking.CreatedAt
            };
        }
    }
}
