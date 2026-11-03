using System;
using TravelAgency.BLL.Exceptions;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.BLL.Internal
{
    /// <summary>
    /// Спільні правила скасування для всіх видів бронювань (DRY):
    /// клієнт скасовує тільки своє і не пізніше дедлайну; менеджер - будь-яке, але до початку.
    /// Лише перевіряє правила і змінює статус - зберігає зміни сервіс.
    /// </summary>
    internal static class BookingCancellationPolicy
    {
        public static void Apply(IBookingEntity booking, User user, DateTime start, DateTime now, int customerDeadlineDays)
        {
            bool isStaff = UserGuard.IsStaff(user);
            if (!isStaff && booking.UserId != user.Id)
            {
                throw new AccessDeniedException("Скасувати можна тільки власне бронювання.");
            }
            if (booking.Status != BookingStatus.Active)
            {
                throw new ConflictException("Бронювання вже скасоване.");
            }
            if (now >= start)
            {
                throw new ConflictException("Подія вже почалася, бронювання не можна скасувати.");
            }
            if (!isStaff && now > start.AddDays(-customerDeadlineDays))
            {
                throw new ConflictException(
                    "Скасувати бронювання можна не пізніше ніж за " + customerDeadlineDays + " дн. до початку.");
            }

            booking.Status = BookingStatus.Cancelled;
        }
    }
}
