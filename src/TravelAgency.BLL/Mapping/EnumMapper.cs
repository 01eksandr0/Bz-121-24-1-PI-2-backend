using Entities = TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.BLL.Mapping
{
    /// <summary>
    /// Перетворення переліків DAL у однойменні переліки BLL і навпаки. Числові значення однакові,
    /// а тест EnumMappingTests гарантує, що вони не розійдуться.
    /// </summary>
    internal static class EnumMapper
    {
        public static Models.UserRole ToModel(this Entities.UserRole role) => (Models.UserRole)(int)role;

        public static Entities.UserRole ToEntity(this Models.UserRole role) => (Entities.UserRole)(int)role;

        public static Models.TourType ToModel(this Entities.TourType type) => (Models.TourType)(int)type;

        public static Entities.TourType ToEntity(this Models.TourType type) => (Entities.TourType)(int)type;

        public static Models.BookingStatus ToModel(this Entities.BookingStatus status) => (Models.BookingStatus)(int)status;

        public static Models.TransportType ToModel(this Entities.TransportType type) => (Models.TransportType)(int)type;

        public static Entities.TransportType ToEntity(this Models.TransportType type) => (Entities.TransportType)(int)type;
    }
}
