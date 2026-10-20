using System;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.DAL.Abstractions.Interfaces
{
    /// <summary>Єдина точка доступу до репозиторіїв і збереження змін однією транзакцією.</summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

        void SaveChanges();
    }
}
