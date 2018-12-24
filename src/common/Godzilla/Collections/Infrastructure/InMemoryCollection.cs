using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task Add(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} already present");

                _innerDict.Add(id, entity);
                return Task.FromResult(true);
            }
        }

        public Task Add<TDerived>(TDerived entity) where TDerived : TItem
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} already present");

                _innerDict.Add(id, entity);
                return Task.FromResult(true);
            }
        }

        public Task Add(IEnumerable<TItem> entities)
        {
            entities
                .ToList()
                .ForEach(x => Add(x).ConfigureAwait(false));

            return Task.FromResult(true);
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

        public Task Delete(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (!_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} not found");

                _innerDict.Remove(id);
                return Task.FromResult(true);
            }
        }
        
        public Task Delete(IEnumerable<TItem> entities)
        {
            entities
                .ToList()
                .ForEach(x => Delete(x).ConfigureAwait(false));

            return Task.FromResult(true);
        }
        
        public Task Update(TItem entity)
        {
            var id = _propertyResolver.GetEntityId(entity);

            lock (this)
            {
                if (!_innerDict.ContainsKey(id))
                    throw new InvalidOperationException($"Element {id} not found");

                _innerDict[id] = entity;
                return Task.FromResult(true);
            }
        }

        public Task Update(IEnumerable<TItem> entities)
        {
            entities
                .ToList()
                .ForEach(x => Update(x).ConfigureAwait(false));

            return Task.FromResult(true);
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
        
        Task IDatabaseCollection<TItem>.Delete(TItem entity)
        {
            throw new NotImplementedException();
        }

        Task IDatabaseCollection<TItem>.Update(TItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
