namespace TravelAgency.BLL.Common
{
    /// <summary>Числові правила предметної області в одному місці (без "магічних чисел").</summary>
    public static class BusinessRules
    {
        public const int MinPasswordLength = 8;
        public const int MaxNameLength = 100;
        public const int MinTitleLength = 5;
        public const int MaxTitleLength = 150;
        public const int MaxDescriptionLength = 4000;
        public const int MaxDiscountPercent = 90;
        public const int MaxTourCapacity = 500;
        public const int MaxPersonsPerBooking = 10;
        public const int MaxSeatsPerBooking = 10;
        public const int MaxHotelNights = 30;
        public const int MinStars = 1;
        public const int MaxStars = 5;
        public const int MaxRoomCapacity = 10;

        /// <summary>Клієнт може скасувати бронювання туру не пізніше ніж за стільки днів до початку.</summary>
        public const int TourCancellationDeadlineDays = 3;
    }
}
