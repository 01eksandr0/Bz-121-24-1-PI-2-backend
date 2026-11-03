using System;
using TravelAgency.BLL.Exceptions;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Internal
{
    internal static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Перевірка вільних місць і запис бронювання - атомарно. Якщо паралельний запит щойно зайняв ті самі місця,
        /// клієнт отримує зрозумілий 409 замість помилки сервера.
        /// </summary>
        public static TResult InTransaction<TResult>(this IUnitOfWork unitOfWork, Func<TResult> action)
        {
            TResult result = default(TResult);
            try
            {
                unitOfWork.ExecuteInTransaction(() => result = action());
            }
            catch (ConcurrencyConflictException)
            {
                throw new ConflictException("Ці місця щойно бронював інший користувач. Спробуйте ще раз.");
            }
            return result;
        }
    }
}
