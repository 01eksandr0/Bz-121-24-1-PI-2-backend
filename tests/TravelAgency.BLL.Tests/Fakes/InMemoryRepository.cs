using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TravelAgency.DAL.Abstractions.Entities;
using TravelAgency.DAL.Abstractions.Interfaces;

namespace TravelAgency.BLL.Tests.Fakes
{
    /// <summary>
    /// Stub-репозиторій у пам'яті. includes ігноруються - навігаційні властивості
    /// заповнює делегат fillReferences (так, як їх підвантажив би EF).
    /// </summary>
    internal class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly List<TEntity> _items = new List<TEntity>();
        private readonly Action<TEntity> _fillReferences;
        private int _nextId = 1;

        public InMemoryRepository(Action<TEntity> fillReferences)
        {
            _fillReferences = fillReferences;
        }

        public int Count => _items.Count;

        /// <summary>Елемент без заповнення зв'язків - для самих делегатів fillReferences (щоб не було рекурсії).</summary>
        public TEntity Raw(int id) => _items.FirstOrDefault(e => e.Id == id);

        public IEnumerable<TEntity> RawAll() => _items;

        public TEntity GetById(int id) => Items().FirstOrDefault(e => e.Id == id);

        public IReadOnlyList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes) => Items().ToList();

        public IReadOnlyList<TEntity> Find(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes) => Items().Where(predicate.Compile()).ToList();

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes) => Items().FirstOrDefault(predicate.Compile());

        public bool Any(Expression<Func<TEntity, bool>> predicate) => Items().Any(predicate.Compile());

        public void Add(TEntity entity)
        {
            entity.Id = _nextId++;
            _items.Add(entity);
        }

        public void Update(TEntity entity)
        {
        }

        public void Remove(TEntity entity)
        {
            _items.Remove(entity);
        }

        private IEnumerable<TEntity> Items()
        {
            _items.ForEach(_fillReferences);
            return _items;
        }
    }
}
