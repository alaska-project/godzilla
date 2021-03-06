﻿using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Collections.Infrastructure
{
    public class InMemoryCollection<TContext, TItem> : IDatabaseCollection<TItem>
        where TContext : EntityContext
    {
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private Dictionary<Guid, TItem> _innerDict = new Dictionary<Guid, TItem>();

        public InMemoryCollection(IEntityPropertyResolver<TContext> propertyResolver, string collectionId)
        {
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            CollectionId = collectionId;
        }

        public string CollectionId { get; }

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

        public Task<TItem> GetItem(Guid id)
        {
            return Task.FromResult(_innerDict.ContainsKey(id) ?
                _innerDict[id] :
                default(TItem));
        }

        public Task<IEnumerable<TItem>> GetItems(IEnumerable<Guid> id)
        {
            return Task.FromResult(_innerDict
                .Where(x => id.Contains(x.Key))
                .Select(x => x.Value));
        }

        public Task<IEnumerable<TItem>> GetItems(IEnumerable<Guid> id, Expression<Func<TItem, bool>> filter)
        {
            var f = filter.Compile();

            return Task.FromResult(_innerDict
                .Where(x => id.Contains(x.Key) && f(x.Value))
                .Select(x => x.Value));
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

        public Task Delete(Expression<Func<TItem, bool>> filter)
        {
            var f = filter.Compile();

            lock(this)
            {
                var elementsToDelete = _innerDict
                    .Where(x => f(x.Value))
                    .ToList();

                elementsToDelete
                    .ForEach(x => _innerDict.Remove(x.Key));
            }

            return Task.FromResult(true);
        }

        public Task Delete(IEnumerable<Guid> id)
        {
            lock (this)
            {
                id
                    .ToList()
                    .ForEach(x => _innerDict.Remove(x));
            }

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

        public Task Update<TField>(TItem entity, Expression<Func<TItem, TField>> field, TField value)
        {
            throw new NotImplementedException();
        }

        public Task Update<TField>(IEnumerable<TItem> entities, Expression<Func<TItem, TField>> field, TField value)
        {
            throw new NotImplementedException();
        }

        public Task Update<TField>(Guid id, Expression<Func<TItem, TField>> field, TField value)
        {
            throw new NotImplementedException();
        }

        public Task Update<TField>(IEnumerable<Guid> idList, Expression<Func<TItem, TField>> field, TField value)
        {
            throw new NotImplementedException();
        }

        public Task CreateIndex(string name, IEnumerable<IIndexField<TItem>> fields, IIndexOptions options)
        {
            return Task.FromResult(true);
        }
    }
}
