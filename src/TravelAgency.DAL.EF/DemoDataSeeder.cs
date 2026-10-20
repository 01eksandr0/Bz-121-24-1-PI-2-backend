using System;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.DAL.EF
{
    /// <summary>
    /// Демонстраційні дані. Дати рахуються від сьогодні, тому тури завжди в майбутньому.
    /// Паролі: admin@travel.ua - Admin123!, manager@travel.ua - Manager123!, olena@mail.com - User123!
    /// (зберігаються лише хеші PBKDF2, обчислені тим самим алгоритмом, що й у BLL).
    /// </summary>
    internal static class DemoDataSeeder
    {
        private const string AdminHash = "PBKDF2-SHA256$100000$VvWIJtRBqT0spergO50/MA==$PP5XSZxi6IBHNhlBNWwVeyfqFDn3Ku1pBKyMMqyAFrQ=";
        private const string ManagerHash = "PBKDF2-SHA256$100000$mDwZw6fzBn1rp/sCEIeXaQ==$OLvNjOUm954OcFrzYIPCoUJ3jN7NQioetlNnM+LDalM=";
        private const string CustomerHash = "PBKDF2-SHA256$100000$CIUXXjMbyXPtAtAc3+nDzg==$MNKgmyZFlKjxar+jj4Kk4OZTqFTubvkYsKVJfLyMGQY=";

        public static void Seed(TravelAgencyContext context)
        {
            DateTime today = DateTime.Today;

            context.Users.AddRange(
                new User { FullName = "Адміністратор", Email = "admin@travel.ua", PasswordHash = AdminHash, Role = UserRole.Admin },
                new User { FullName = "Марія Менеджер", Email = "manager@travel.ua", PasswordHash = ManagerHash, Role = UserRole.Manager },
                new User { FullName = "Олена Клієнт", Email = "olena@mail.com", PasswordHash = CustomerHash, Role = UserRole.Customer });

            var turkey = new Country { Name = "Туреччина" };
            var italy = new Country { Name = "Італія" };
            var ukraine = new Country { Name = "Україна" };
            var egypt = new Country { Name = "Єгипет" };
            context.Countries.AddRange(turkey, italy, ukraine, egypt);

            var antalya = new Region { Name = "Анталія", Country = turkey };
            var istanbul = new Region { Name = "Стамбул", Country = turkey };
            var rome = new Region { Name = "Рим", Country = italy };
            var dolomites = new Region { Name = "Доломітові Альпи", Country = italy };
            var carpathians = new Region { Name = "Карпати", Country = ukraine };
            var lviv = new Region { Name = "Львів", Country = ukraine };
            var hurghada = new Region { Name = "Хургада", Country = egypt };
            context.Regions.AddRange(antalya, istanbul, rome, dolomites, carpathians, lviv, hurghada);

            context.Tours.AddRange(
                NewTour("Анталія: все включено", "Готель 5*, перша лінія, власний пляж.", TourType.Beach, antalya, today.AddDays(7), 7, 950m, 20, 30),
                NewTour("Стамбул за 4 дні", "Екскурсії Айя-Софія, Блакитна мечеть, Босфор.", TourType.Excursion, istanbul, today.AddDays(21), 4, 520m, 0, 20),
                NewTour("Вічне місто Рим", "Колізей, Ватикан, Римський форум з гідом.", TourType.Excursion, rome, today.AddDays(35), 5, 890m, 0, 25),
                NewTour("Лижі в Доломітах", "Ски-пас, інструктор, шале біля підйомника.", TourType.Ski, dolomites, today.AddDays(60), 7, 1450m, 0, 12),
                NewTour("Буковель вихідного дня", "Трансфер з Києва, проживання, ски-пас.", TourType.Ski, carpathians, today.AddDays(10), 3, 320m, 15, 40),
                NewTour("Старий Львів", "Пішохідна екскурсія, кав'ярні, підземелля.", TourType.Excursion, lviv, today.AddDays(5), 2, 150m, 10, 30),
                NewTour("Хургада: дайвінг", "Червоне море, 3 занурення з інструктором.", TourType.Beach, hurghada, today.AddDays(14), 8, 780m, 0, 24),
                NewTour("Трускавець: оздоровлення", "Мінеральні води, процедури, харчування.", TourType.Health, carpathians, today.AddDays(28), 10, 640m, 0, 15));

            var rixos = new Hotel { Name = "Rixos Downtown", Description = "Пляжний готель, басейни, спа.", Stars = 5, Region = antalya };
            rixos.Rooms.Add(new HotelRoom { Name = "Стандарт (2 особи)", Capacity = 2, PricePerNight = 120m });
            rixos.Rooms.Add(new HotelRoom { Name = "Сімейний (4 особи)", Capacity = 4, PricePerNight = 210m });
            var roma = new Hotel { Name = "Hotel Roma Centro", Description = "5 хвилин до Колізею.", Stars = 4, Region = rome };
            roma.Rooms.Add(new HotelRoom { Name = "Одномісний", Capacity = 1, PricePerNight = 95m });
            roma.Rooms.Add(new HotelRoom { Name = "Двомісний", Capacity = 2, PricePerNight = 140m });
            var bukovel = new Hotel { Name = "Буковель Шале", Description = "Біля підйомника №1.", Stars = 3, Region = carpathians };
            bukovel.Rooms.Add(new HotelRoom { Name = "Двомісний", Capacity = 2, PricePerNight = 60m });
            context.Hotels.AddRange(rixos, roma, bukovel);

            context.TransportTrips.AddRange(
                NewTrip(TransportType.Plane, "Київ", "Анталія", today.AddDays(7).AddHours(9), 3, 180m, 150),
                NewTrip(TransportType.Plane, "Київ", "Рим", today.AddDays(35).AddHours(7), 3, 210m, 120),
                NewTrip(TransportType.Train, "Київ", "Львів", today.AddDays(5).AddHours(6), 5, 25m, 300),
                NewTrip(TransportType.Bus, "Київ", "Буковель", today.AddDays(10).AddHours(22), 10, 30m, 45));

            context.SaveChanges();
        }

        private static Tour NewTour(string title, string description, TourType type, Region region,
            DateTime start, int nights, decimal price, int discountPercent, int capacity)
        {
            return new Tour
            {
                Title = title,
                Description = description,
                Type = type,
                Region = region,
                StartDate = start,
                EndDate = start.AddDays(nights),
                PricePerPerson = price,
                DiscountPercent = discountPercent,
                Capacity = capacity
            };
        }

        private static TransportTrip NewTrip(TransportType type, string from, string to,
            DateTime departure, int durationHours, decimal price, int seats)
        {
            return new TransportTrip
            {
                Type = type,
                FromCity = from,
                ToCity = to,
                DepartureAt = departure,
                ArrivalAt = departure.AddHours(durationHours),
                Price = price,
                SeatsTotal = seats
            };
        }
    }
}
