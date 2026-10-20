using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;
using TravelAgency.DAL.EF.Repositories;

namespace TravelAgency.DAL.EF
{
    /// <summary>Unit of Work: один контекст для всіх репозиторіїв і одне збереження змін.</summary>
    public class EfUnitOfWork : IUnitOfWork
    {
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
