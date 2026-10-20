using TravelAgency.BLL.Models;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.BLL.Mapping
{
    /// <summary>
    /// Перетворення переліків DAL у переліки BLL і навпаки. Числові значення однакові,
    /// а тест EnumMappingTests гарантує, що вони не розійдуться.
    /// </summary>
    internal static class EnumMapper
    {
        public static Role ToModel(this UserRole role) => (Role)(int)role;

        public static UserRole ToEntity(this Role role) => (UserRole)(int)role;

        public static TourKind ToModel(this TourType type) => (TourKind)(int)type;

        public static TourType ToEntity(this TourKind type) => (TourType)(int)type;

        public static BookingState ToModel(this BookingStatus status) => (BookingState)(int)status;

        public static TransportKind ToModel(this TransportType type) => (TransportKind)(int)type;

        public static TransportType ToEntity(this TransportKind type) => (TransportType)(int)type;
    }
}
