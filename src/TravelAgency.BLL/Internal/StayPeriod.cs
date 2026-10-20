using System;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Exceptions;

namespace TravelAgency.BLL.Internal
{
    /// <summary>Перевірка дат проживання в готелі (використовують HotelService і RoomBookingService).</summary>
    internal static class StayPeriod
    {
        public static int Validate(DateTime checkIn, DateTime checkOut, DateTime today)
        {
            if (checkIn.Date < today)
            {
                throw new BusinessValidationException("Дата заїзду не може бути в минулому.");
            }
            int nights = (checkOut.Date - checkIn.Date).Days;
            if (nights <= 0)
            {
                throw new BusinessValidationException("Дата виїзду має бути пізніше за дату заїзду.");
            }
            if (nights > BusinessRules.MaxHotelNights)
            {
                throw new BusinessValidationException("Максимальний термін проживання - " + BusinessRules.MaxHotelNights + " ночей.");
            }
            return nights;
        }
    }
}
