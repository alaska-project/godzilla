using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Collections.Infrastructure
{
    public class InMemoryCollection<TContext, TItem> : IDatabaseCollection<TItem>
        where TContext : EntityContext
    {
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private Dictionary<Guid, TItem> _innerDict = new Dictionary<Guid, TItem>();

        public InMemoryCollection(IEntityPropertyResolver<TContext> propertyResolver)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
        }

        public void Add(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} already present");

                _innerDict.Add(id, entity);
            }
        }

        public void Add<TDerived>(TDerived entity) where TDerived : TItem
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} already present");

                _innerDict.Add(id, entity);
            }
        }

        public IQueryable<TItem> AsQueryable()
        {
            return _innerDict.Values.AsQueryable();
        }

        public IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : TItem
        {
            return _innerDict
                .Values
                .Where(x => x is TDerived)
                .Cast<TDerived>()
                .AsQueryable();
        }

        public void Delete(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (!_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} not found");

                _innerDict.Remove(id);
            }
        }

        public void Delete<TDerived>(TItem entity) where TDerived : TItem
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (!_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} not found");

                _innerDict.Remove(id);
            }
        }

        public void Update(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (!_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} not found");

                _innerDict[id] = entity;
            }
        }

        public void Update<TDerived>(TDerived entity) where TDerived : TItem
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (!_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} not found");

                _innerDict[id] = entity;
            }
        }
    }
}
