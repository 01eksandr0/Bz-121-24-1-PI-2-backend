using System;
using System.Collections.Generic;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.DAL.EF.Repositories
{
    /// <summary>Створює і кешує репозиторії одного контексту (винесено з UnitOfWork).</summary>
    internal class EfRepositoryProvider
    {
        private readonly DbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public EfRepositoryProvider(DbContext context)
        {
            _context = context;
        }

        public IRepository<TEntity> Get<TEntity>() where TEntity : class, IEntity
        {
            object repository;
            if (!_repositories.TryGetValue(typeof(TEntity), out repository))
            {
                repository = new EfRepository<TEntity>(_context);
                _repositories.Add(typeof(TEntity), repository);
            }
            return (IRepository<TEntity>)repository;
        }
    }
}
