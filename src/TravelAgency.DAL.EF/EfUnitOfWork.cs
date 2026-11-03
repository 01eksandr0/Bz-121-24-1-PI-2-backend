using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;
using TravelAgency.DAL.EF.Repositories;

namespace TravelAgency.DAL.EF
{
    /// <summary>
    /// Unit of Work: один контекст для всіх репозиторіїв і одне збереження змін.
    /// Контекст створює і звільняє DI-контейнер, тому UoW його не закриває.
    /// </summary>
    internal class EfUnitOfWork : IUnitOfWork
    {
        private const int DeadlockErrorNumber = 1205;

        private readonly TravelAgencyContext _context;
        private readonly EfRepositoryProvider _repositories;

        public EfUnitOfWork(TravelAgencyContext context)
        {
            _context = context;
            _repositories = new EfRepositoryProvider(context);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            return _repositories.Get<TEntity>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Serializable: прочитані рядки (зайняті місця) блокуються до кінця транзакції, тож два запити
        /// не можуть одночасно побачити "є місце" і обидва його зайняти. Програв блокування - ConcurrencyConflictException.
        /// </summary>
        public void ExecuteInTransaction(Action action)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    action();
                    transaction.Commit();
                }
            }
            catch (Exception exception) when (IsDeadlock(exception))
            {
                _context.ChangeTracker.Clear();
                throw new ConcurrencyConflictException(exception);
            }
        }

        private static bool IsDeadlock(Exception exception)
        {
            SqlException sqlException = exception as SqlException ?? exception.InnerException as SqlException;
            return sqlException != null && sqlException.Number == DeadlockErrorNumber;
        }
    }
}
