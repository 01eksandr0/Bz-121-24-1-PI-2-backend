using TravelAgency.DAL.Abstractions;

namespace TravelAgency.BLL.Common
{
    /// <summary>Числові правила предметної області в одному місці (без "магічних чисел").</summary>
    internal static class BusinessRules
    {
        public const int MinPasswordLength = 8;
        public const int MaxNameLength = EntityLimits.NameLength;
        public const int MinTitleLength = 5;
        public const int MaxTitleLength = EntityLimits.TitleLength;
        public const int MaxDescriptionLength = EntityLimits.DescriptionLength;
        public const int MaxDiscountPercent = 90;

        /// <summary>Мінімальна кількість осіб, місць, гостей чи номерів - хоча б один.</summary>
        public const int MinQuantity = 1;

        public const int MaxTourCapacity = 500;
        public const int MaxTripCapacity = 500;
        public const int MaxPersonsPerBooking = 10;
        public const int MaxSeatsPerBooking = 10;
        public const int MaxHotelNights = 30;
        public const int MinStars = 1;
        public const int MaxStars = 5;
        public const int MaxRoomCapacity = 10;
        public const int MaxRoomQuantity = 200;

        /// <summary>Клієнт може скасувати бронювання туру не пізніше ніж за стільки днів до початку.</summary>
        public const int TourCancellationDeadlineDays = 3;

        /// <summary>Номер у готелі клієнт може скасувати до дня заїзду.</summary>
        public const int RoomCancellationDeadlineDays = 0;

        /// <summary>Квиток клієнт може скасувати до відправлення.</summary>
        public const int TicketCancellationDeadlineDays = 0;
    }
}
