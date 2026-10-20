using System;
using TravelAgency.BLL.Exceptions;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Internal
{
    /// <summary>
    /// Спільні правила скасування для всіх видів бронювань (DRY):
    /// клієнт скасовує тільки своє і не пізніше дедлайну; менеджер - будь-яке, але до початку.
    /// </summary>
    internal static class BookingCancellation
    {
        public static void Cancel<TBooking>(IUnitOfWork unitOfWork, TBooking booking, User user,
            DateTime start, DateTime now, int customerDeadlineDays) where TBooking : class, IBookingEntity
        {
            bool isStaff = UserGuard.IsStaff(user);
            if (!isStaff && booking.UserId != user.Id)
            {
                throw new AccessDeniedException("Скасувати можна тільки власне бронювання.");
            }
            if (booking.Status != BookingStatus.Active)
            {
                throw new BusinessConflictException("Бронювання вже скасоване.");
            }
            if (now >= start)
            {
                throw new BusinessConflictException("Подія вже почалася, бронювання не можна скасувати.");
            }
            if (!isStaff && now > start.AddDays(-customerDeadlineDays))
            {
                throw new BusinessConflictException(
                    "Скасувати бронювання можна не пізніше ніж за " + customerDeadlineDays + " дн. до початку.");
            }

            booking.Status = BookingStatus.Cancelled;
            unitOfWork.Repository<TBooking>().Update(booking);
            unitOfWork.SaveChanges();
        }
    }
}
