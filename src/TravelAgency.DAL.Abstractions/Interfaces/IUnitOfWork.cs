using System;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Єдина точка доступу до репозиторіїв і збереження змін.
    /// Час життя (і звільнення ресурсів) визначає DI-контейнер, тому IDisposable тут немає.
    /// </summary>
    public interface IUnitOfWork
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

        void SaveChanges();

        /// <summary>
        /// Виконує дію (перевірка + запис) в одній ізольованій транзакції, щоб паралельні запити
        /// не продали одне й те саме місце двічі. Якщо паралельний запит завадив - ConcurrencyConflictException.
        /// </summary>
        void ExecuteInTransaction(Action action);
    }
}
