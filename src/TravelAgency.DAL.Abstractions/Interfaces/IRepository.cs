using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TravelAgency.DAL.Abstractions.Entities;

namespace TravelAgency.DAL.Abstractions.Interfaces
{
    /// <summary>
    /// Один generic-репозиторій для всіх сутностей.
    /// includes - пов'язані дані, які треба підвантажити разом із сутністю.
    /// </summary>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity GetById(int id);

        IReadOnlyList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        IReadOnlyList<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        TEntity FirstOrDefault(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);
    }
}
