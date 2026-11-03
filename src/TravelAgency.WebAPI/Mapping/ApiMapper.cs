using System;
using System.Collections.Generic;
using System.Linq;
using TravelAgency.BLL.Models;
using TravelAgency.BLL.Models.Bookings;
using TravelAgency.BLL.Models.Countries;
using TravelAgency.BLL.Models.Hotels;
using TravelAgency.BLL.Models.Tours;
using TravelAgency.BLL.Models.Trips;
using TravelAgency.BLL.Models.Users;
using TravelAgency.WebAPI.Models.Requests;
using TravelAgency.WebAPI.Models.Responses;

namespace TravelAgency.WebAPI.Mapping
{
    /// <summary>
    /// Mapping між моделями PL (запити/відповіді) і моделями BLL.
    /// Переліки передаються клієнту рядками ("Beach", "Active"), тому PL не залежить від їх числових значень.
    /// Значення обов'язкових полів (.Value) уже перевірені атрибутами [Required] до виклику контролера.
    /// </summary>
    internal static class ApiMapper
    {
        public static UserResponse ToResponse(this UserModel user) => new UserResponse
        {
            Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role.ToString()
        };

        public static RegionResponse ToResponse(this RegionModel region) => new RegionResponse
        {
            Id = region.Id, Name = region.Name, CountryId = region.CountryId, CountryName = region.CountryName
        };

        public static CountryResponse ToResponse(this CountryModel country) => new CountryResponse
        {
            Id = country.Id, Name = country.Name, Regions = country.Regions.Select(r => r.ToResponse()).ToList()
        };

        public static TourResponse ToResponse(this TourModel tour) => new TourResponse
        {
            Id = tour.Id,
            Title = tour.Title,
            Description = tour.Description,
            Type = tour.Type.ToString(),
            RegionId = tour.RegionId,
            RegionName = tour.RegionName,
            CountryId = tour.CountryId,
            CountryName = tour.CountryName,
            StartDate = tour.StartDate,
            EndDate = tour.EndDate,
            Nights = tour.Nights,
            PricePerPerson = tour.PricePerPerson,
            DiscountPercent = tour.DiscountPercent,
            FinalPrice = tour.FinalPrice,
            IsHot = tour.IsHot,
            Capacity = tour.Capacity,
            FreePlaces = tour.FreePlaces
        };

        public static TourBookingResponse ToResponse(this TourBookingModel booking) => new TourBookingResponse
        {
            Id = booking.Id,
            TourId = booking.TourId,
            TourTitle = booking.TourTitle,
            TourStartDate = booking.TourStartDate,
            UserName = booking.UserName,
            Persons = booking.Persons,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            CreatedAt = booking.CreatedAt
        };

        public static RoomBookingResponse ToResponse(this RoomBookingModel booking) => new RoomBookingResponse
        {
            Id = booking.Id,
            RoomId = booking.RoomId,
            HotelName = booking.HotelName,
            RoomName = booking.RoomName,
            UserName = booking.UserName,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            Nights = booking.Nights,
            Guests = booking.Guests,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            CreatedAt = booking.CreatedAt
        };

        public static TicketBookingResponse ToResponse(this TicketBookingModel booking) => new TicketBookingResponse
        {
            Id = booking.Id,
            TripId = booking.TripId,
            Route = booking.Route,
            TransportType = booking.TransportType.ToString(),
            DepartureAt = booking.DepartureAt,
            UserName = booking.UserName,
            Seats = booking.Seats,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status.ToString(),
            CreatedAt = booking.CreatedAt
        };

        public static RoomResponse ToResponse(this RoomModel room) => new RoomResponse
        {
            Id = room.Id, Name = room.Name, Capacity = room.Capacity, Quantity = room.Quantity, PricePerNight = room.PricePerNight
        };

        public static HotelResponse ToResponse(this HotelModel hotel) => new HotelResponse
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Description = hotel.Description,
            Stars = hotel.Stars,
            RegionId = hotel.RegionId,
            RegionName = hotel.RegionName,
            CountryName = hotel.CountryName,
            MinPricePerNight = hotel.MinPricePerNight,
            Rooms = hotel.Rooms.Select(r => r.ToResponse()).ToList()
        };

        public static TripResponse ToResponse(this TripModel trip) => new TripResponse
        {
            Id = trip.Id,
            Type = trip.Type.ToString(),
            FromCity = trip.FromCity,
            ToCity = trip.ToCity,
            DepartureAt = trip.DepartureAt,
            ArrivalAt = trip.ArrivalAt,
            Price = trip.Price,
            Capacity = trip.Capacity,
            FreePlaces = trip.FreePlaces
        };

        public static List<TResponse> ToResponses<TModel, TResponse>(this IEnumerable<TModel> models, Func<TModel, TResponse> map)
        {
            return models.Select(map).ToList();
        }

        public static RegisterInputModel ToModel(this RegisterRequest request) => new RegisterInputModel
        {
            FullName = request.FullName, Email = request.Email, Password = request.Password
        };

        public static LoginInputModel ToModel(this LoginRequest request) => new LoginInputModel
        {
            Email = request.Email, Password = request.Password
        };

        public static TourInputModel ToModel(this TourRequest request) => new TourInputModel
        {
            Title = request.Title,
            Description = request.Description,
            Type = ParseEnum<TourType>(request.Type),
            RegionId = request.RegionId.Value,
            StartDate = request.StartDate.Value,
            EndDate = request.EndDate.Value,
            PricePerPerson = request.PricePerPerson.Value,
            DiscountPercent = request.DiscountPercent,
            Capacity = request.Capacity.Value
        };

        public static TourSearchModel ToModel(this TourSearchQuery query) => new TourSearchModel
        {
            Text = query.Text,
            CountryId = query.CountryId,
            RegionId = query.RegionId,
            Type = string.IsNullOrEmpty(query.Type) ? (TourType?)null : ParseEnum<TourType>(query.Type),
            HotOnly = query.HotOnly,
            MinPrice = query.MinPrice,
            MaxPrice = query.MaxPrice,
            DateFrom = query.DateFrom,
            DateTo = query.DateTo,
            OnlyAvailable = query.OnlyAvailable,
            SortBy = string.IsNullOrEmpty(query.SortBy) ? TourSortOrder.StartDate : ParseEnum<TourSortOrder>(query.SortBy)
        };

        public static TourBookingInputModel ToModel(this TourBookingRequest request) => new TourBookingInputModel
        {
            TourId = request.TourId.Value, Persons = request.Persons.Value
        };

        public static RoomBookingInputModel ToModel(this RoomBookingRequest request) => new RoomBookingInputModel
        {
            RoomId = request.RoomId.Value, CheckIn = request.CheckIn.Value, CheckOut = request.CheckOut.Value, Guests = request.Guests.Value
        };

        public static TicketBookingInputModel ToModel(this TicketBookingRequest request) => new TicketBookingInputModel
        {
            TripId = request.TripId.Value, Seats = request.Seats.Value
        };

        public static HotelInputModel ToModel(this HotelRequest request) => new HotelInputModel
        {
            Name = request.Name, Description = request.Description, Stars = request.Stars.Value, RegionId = request.RegionId.Value
        };

        public static RoomInputModel ToModel(this RoomRequest request) => new RoomInputModel
        {
            Name = request.Name, Capacity = request.Capacity.Value, Quantity = request.Quantity.Value, PricePerNight = request.PricePerNight.Value
        };

        public static HotelSearchModel ToModel(this HotelSearchQuery query) => new HotelSearchModel
        {
            CountryId = query.CountryId, RegionId = query.RegionId, MinStars = query.MinStars, Text = query.Text
        };

        public static TripInputModel ToModel(this TripRequest request) => new TripInputModel
        {
            Type = ParseEnum<TransportType>(request.Type),
            FromCity = request.FromCity,
            ToCity = request.ToCity,
            DepartureAt = request.DepartureAt.Value,
            ArrivalAt = request.ArrivalAt.Value,
            Price = request.Price.Value,
            Capacity = request.Capacity.Value
        };

        public static TripSearchModel ToModel(this TripSearchQuery query) => new TripSearchModel
        {
            Type = string.IsNullOrEmpty(query.Type) ? (TransportType?)null : ParseEnum<TransportType>(query.Type),
            FromCity = query.FromCity,
            ToCity = query.ToCity,
            Date = query.Date
        };

        public static UserRole ToRole(this ChangeRoleRequest request) => ParseEnum<UserRole>(request.Role);

        /// <summary>Рядок уже перевірено атрибутом [EnumDataType], тому тут лише перетворення.</summary>
        private static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value, true);
        }
    }
}
