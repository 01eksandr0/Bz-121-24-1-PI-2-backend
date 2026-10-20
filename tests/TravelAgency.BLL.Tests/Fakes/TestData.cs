using System;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.BLL.Tests.Fakes
{
    /// <summary>
    /// Набір даних для тестів (свіжий для кожного тесту).
    /// Користувачі: 1 Олена (клієнт), 2 Петро (клієнт), 3 Марія (менеджер), 4 Адмін.
    /// Країни: 1 Туреччина (регіон 1 Анталія), 2 Італія (регіон 2 Рим).
    /// Тури: 1 Анталія через 10 днів, 20 місць, знижка 20%; 2 Рим через 2 дні, 2 місця; 3 Рим - минулий.
    /// Готель 1 в Анталії: номер 1 (2 особи, 100/ніч), номер 2 (4 особи, 180/ніч).
    /// Рейс 1 Київ - Анталія через 10 днів, 3 місця.
    /// </summary>
    internal static class TestData
    {
        public const int Olena = 1;
        public const int Petro = 2;
        public const int Manager = 3;
        public const int Admin = 4;
        public const int UnknownUser = 99;

        public const int Turkey = 1;
        public const int Antalya = 1;
        public const int Rome = 2;

        public const int HotTour = 1;
        public const int SmallTour = 2;
        public const int PastTour = 3;

        public const int Hotel = 1;
        public const int DoubleRoom = 1;
        public const int FamilyRoom = 2;

        public const int Flight = 1;

        public static DateTime Today => FixedClock.FixedNow.Date;

        public static InMemoryUnitOfWork Create()
        {
            var uow = new InMemoryUnitOfWork();
            uow.Users.Add(new User { FullName = "Олена", Email = "olena@mail.com", PasswordHash = "hash:User123!", Role = UserRole.Customer });
            uow.Users.Add(new User { FullName = "Петро", Email = "petro@mail.com", PasswordHash = "hash:User123!", Role = UserRole.Customer });
            uow.Users.Add(new User { FullName = "Марія", Email = "manager@travel.ua", PasswordHash = "hash:Manager123!", Role = UserRole.Manager });
            uow.Users.Add(new User { FullName = "Адмін", Email = "admin@travel.ua", PasswordHash = "hash:Admin123!", Role = UserRole.Admin });

            uow.Countries.Add(new Country { Name = "Туреччина" });
            uow.Countries.Add(new Country { Name = "Італія" });
            uow.Regions.Add(new Region { Name = "Анталія", CountryId = 1 });
            uow.Regions.Add(new Region { Name = "Рим", CountryId = 2 });

            uow.Tours.Add(NewTour("Анталія все включено", TourType.Beach, Antalya, 10, 7, 1000m, 20, 20));
            uow.Tours.Add(NewTour("Рим на вихідні", TourType.Excursion, Rome, 2, 3, 500m, 0, 2));
            uow.Tours.Add(NewTour("Минулий тур до Риму", TourType.Excursion, Rome, -5, 3, 400m, 0, 10));

            uow.Hotels.Add(new Hotel { Name = "Sea Hotel", Stars = 5, RegionId = Antalya });
            uow.Rooms.Add(new HotelRoom { HotelId = Hotel, Name = "Двомісний", Capacity = 2, PricePerNight = 100m });
            uow.Rooms.Add(new HotelRoom { HotelId = Hotel, Name = "Сімейний", Capacity = 4, PricePerNight = 180m });

            uow.Trips.Add(new TransportTrip
            {
                Type = TransportType.Plane, FromCity = "Київ", ToCity = "Анталія",
                DepartureAt = Today.AddDays(10).AddHours(9), ArrivalAt = Today.AddDays(10).AddHours(12),
                Price = 150m, SeatsTotal = 3
            });
            return uow;
        }

        private static Tour NewTour(string title, TourType type, int regionId, int startInDays, int nights,
            decimal price, int discount, int capacity)
        {
            return new Tour
            {
                Title = title, Description = "Опис", Type = type, RegionId = regionId,
                StartDate = Today.AddDays(startInDays), EndDate = Today.AddDays(startInDays + nights),
                PricePerPerson = price, DiscountPercent = discount, Capacity = capacity
            };
        }
    }
}
