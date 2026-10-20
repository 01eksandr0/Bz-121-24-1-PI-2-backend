using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.DAL.EF.Repositories
{
    /// <summary>Generic-репозиторій на EF Core: працює з будь-якою сутністю через DbSet.</summary>
    internal class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _set;

        public EfRepository(DbContext context)
        {
            _context = context;
            _set = context.Set<TEntity>();
        }

        public TEntity GetById(int id)
        {
            return _set.Find(id);
        }

        public IReadOnlyList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return WithIncludes(includes).ToList();
        }

        public IReadOnlyList<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return WithIncludes(includes).Where(predicate).ToList();
        }

        public TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return WithIncludes(includes).FirstOrDefault(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Any(predicate);
        }

        public void Add(TEntity entity)
        {
            _set.Add(entity);
        }

        public void Update(TEntity entity)
        {
            // відстежувану сутність EF оновить сам; від'єднану треба приєднати
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _set.Update(entity);
            }
        }

        public void Remove(TEntity entity)
        {
            _set.Remove(entity);
        }

        private IQueryable<TEntity> WithIncludes(IEnumerable<Expression<Func<TEntity, object>>> includes)
        {
            IQueryable<TEntity> query = _set;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }
    }
}
