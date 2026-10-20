using System;
using System.Linq.Expressions;
using TravelAgency.BLL.Exceptions;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Internal
{
    /// <summary>"Знайти або кинути 404" - щоб не повторювати перевірку на null у кожному сервісі.</summary>
    internal static class RepositoryExtensions
    {
        public static TEntity GetRequired<TEntity>(this IRepository<TEntity> repository, int id, string entityName,
            params Expression<Func<TEntity, object>>[] includes) where TEntity : class, IEntity
        {
            TEntity entity = includes.Length == 0
                ? repository.GetById(id)
                : repository.FirstOrDefault(e => e.Id == id, includes);
            if (entity == null)
            {
                throw new EntityNotFoundException(entityName, id);
            }
            return entity;
        }
    }
}
