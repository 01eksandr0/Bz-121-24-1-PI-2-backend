using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TravelAgency.BLL.Common;
using TravelAgency.BLL.Internal;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Services
{
    /// <summary>
    /// Шаблонний метод для трьох видів бронювань: перегляд, скасування і збереження нового бронювання
    /// однакові, а відрізняються лише деталі, які задає кожен нащадок (що підвантажити, коли подія починається тощо).
    /// </summary>
    internal abstract class BookingServiceBase<TBooking, TModel> where TBooking : class, IBookingEntity
    {
        protected BookingServiceBase(IUnitOfWork unitOfWork, IClock clock)
        {
            UnitOfWork = unitOfWork;
            Clock = clock;
        }

        protected IUnitOfWork UnitOfWork { get; }

        protected IClock Clock { get; }

        protected IRepository<TBooking> Bookings => UnitOfWork.Repository<TBooking>();

        /// <summary>Назва для повідомлення "не знайдено".</summary>
        protected abstract string EntityName { get; }

        /// <summary>Пов'язані дані, потрібні для ToModel.</summary>
        protected abstract Expression<Func<TBooking, object>>[] Details { get; }

        /// <summary>За скільки днів до початку клієнт ще може скасувати.</summary>
        protected abstract int CancellationDeadlineDays { get; }

        /// <summary>Поточний момент для правила скасування: дата для подій "по днях", точний час для рейсів.</summary>
        protected virtual DateTime CancellationNow => Clock.Today;

        public IReadOnlyList<TModel> GetByUser(int userId)
        {
            return ToSortedModels(Bookings.FindAll(b => b.UserId == userId, Details));
        }

        public IReadOnlyList<TModel> GetAll()
        {
            return ToSortedModels(Bookings.GetAll(Details));
        }

        public void Cancel(int bookingId, int userId)
        {
            User user = UserGuard.GetRegistered(UnitOfWork, userId);
            TBooking booking = Bookings.GetRequired(bookingId, EntityName, Details);
            BookingCancellationPolicy.Apply(booking, user, StartOf(booking), CancellationNow, CancellationDeadlineDays);
            Bookings.Update(booking);
            UnitOfWork.SaveChanges();
        }

        /// <summary>Початок заброньованої події (потребує Details).</summary>
        protected abstract DateTime StartOf(TBooking booking);

        /// <summary>Модель для відповіді (потребує Details).</summary>
        protected abstract TModel ToModel(TBooking booking);

        /// <summary>Записує нове активне бронювання. Викликається всередині транзакції разом з перевіркою місць.</summary>
        protected TBooking Add(TBooking booking)
        {
            booking.Status = BookingStatus.Active;
            booking.CreatedAt = Clock.Now;
            Bookings.Add(booking);
            UnitOfWork.SaveChanges();
            return booking;
        }

        /// <summary>Перечитує бронювання з усіма Details для відповіді.</summary>
        protected TModel Reload(TBooking booking)
        {
            return ToModel(Bookings.GetRequired(booking.Id, EntityName, Details));
        }

        private IReadOnlyList<TModel> ToSortedModels(IEnumerable<TBooking> bookings)
        {
            return bookings.OrderByDescending(b => b.CreatedAt).Select(ToModel).ToList();
        }
    }
}
