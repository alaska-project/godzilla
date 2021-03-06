﻿using Godzilla.Abstractions;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla
{
    public class Document<TEntity>
    {
        #region Init

        private readonly EntityContext _context;
        private TEntity _entity;

        internal Document(EntityContext context, TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entity = entity;
            Id = context.Resolver.GetEntityId(entity);
        }

        internal EntityContext Context => _context;

        public Guid Id { get; }
        public TEntity Value => _entity;

        #endregion

        #region Commands

        public async Task SetPermissions(RuleSubject subject, IEnumerable<SecurityRule> rules)
        {
            ChekId(_entity);

            await _context.Commands.SetEntityPermissions(Id, subject, rules);
        }

        public async Task ClearPermissions(RuleSubject subject)
        {
            ChekId(_entity);

            await _context.Commands.ClearEntityPermissions(Id, subject);
        }

        public async Task Delete()
        {
            ChekId(_entity);

            await _context.Commands.Delete(_entity);

            _entity = default(TEntity);
        }

        public async Task Set(TEntity value)
        {
            ChekId(value);

            _entity = value;

            await Update();
        }

        public async Task UpdateField<TField>(Expression<Func<TEntity, TField>> field, TField value)
        {
            ChekId(_entity);

            _entity = await _context.Commands.Update(_entity, field, value);
        }

        public async Task ClearField<TField>(Expression<Func<TEntity, TField>> field)
        {
            await UpdateField(field, default(TField));
        }

        public async Task Update()
        {
            ChekId(_entity);

            _entity = await _context.Commands.Update(_entity);
        }

        public async Task Rename(string newName)
        {
            ChekId(_entity);

            await _context.Commands.Rename(_entity, newName);
        }

        public async Task<Document<TChild>> AddChild<TChild>(TChild child)
        {
            return await AddChild(null, child);
        }

        public async Task<Document<TChild>> AddChild<TChild>(string name, TChild child)
        {
            ChekId(_entity);

            var entity = await _context.Commands.Add(child, _entity);

            if (!string.IsNullOrEmpty(name))
                await _context.Commands.Rename(entity, name);

            return CreateDocument(entity);
        }

        public async Task<IEnumerable<Document<TChild>>> AddChildren<TChild>(IEnumerable<TChild> children)
        {
            ChekId(_entity);

            var entities = await _context.Commands.Add(children, _entity);
            return CreateDocuments(entities);
        }

        public async Task<DocumentContainer> Container(string name)
        {
            var container = await _context.Query.GetChild<Container>(_entity, x => x.Name.ToLower() == name.ToLower());
            if (container == null)
                container = await _context.Commands.Add(new Container
                {
                    Name = name
                }, _entity);

            return new DocumentContainer(_context, container);
        }

        public async Task<Document<T>> Container<T>(string path)
        {
            return await Container<T>(path, () => Activator.CreateInstance<T>());
        }

        public async Task<Document<T>> Container<T>(string path, Func<T> valueFactory)
        {
            var segments = Context.PathBuilder.GetSegments(path);
            if (!segments.Any())
                throw new ArgumentException("Empty path not allowed");

            // Container with subcontainers
            if (segments.Count() > 1)
            {
                Document<T> container = null;
                foreach (var segment in segments)
                {
                    if (container == null)
                        container = await Container<T>(segment, valueFactory);
                    else
                        container = await container.Container<T>(segment, valueFactory);
                }
                return container;
            }
            else
            {
                var name = segments.First();
                var container = await _context.Query.GetChild<T>(_entity, name);
                if (container == null)
                {
                    var value = valueFactory();
                    container = await _context.Commands.Add(value, _entity);
                    await _context.Commands.Rename(container, name);
                }

                return new Document<T>(_context, container);
            }
        }

        #endregion

        #region Query

        public async Task<Document<T>> GetParent<T>()
        {
            var parent = await _context.Query.GetParent<T>(_entity);

            return parent == null ? 
                null : 
                CreateDocument(parent);
        }

        public async Task<Document<T>> GetOrCreateChild<T>(string name, Func<T> childCreator)
        {
            var child = await GetChild<T>(name);
            if (child != null)
                return child;

            var childValue = childCreator();
            return await AddChild(name, childValue);
        }

        public async Task<Document<T>> GetOrCreateChild<T>(Func<T> childCreator)
        {
            var child = await GetChild<T>();
            if (child != null)
                return child;

            var childValue = childCreator();
            return await AddChild(childValue);
        }
        
        public async Task<Document<T>> GetChild<T>()
        {
            var children = await GetChildren<T>();
            return children.FirstOrDefault();
        }

        public async Task<Document<T>> GetChild<T>(string name)
        {
            var children = await GetChildren<T>(name);
            return children.FirstOrDefault();
        }

        public async Task<Document<T>> GetChild<T>(Expression<Func<T, bool>> filter)
        {
            var children = await GetChildren<T>(filter);
            return children.FirstOrDefault();
        }

        public async Task<Document<T>> GetChild<T>(Guid entityId)
        {
            var children = await GetChildren<T>(new List<Guid> { entityId });
            return children.FirstOrDefault();
        }

        public async Task<IEnumerable<Document<T>>> GetChildren<T>(string name)
        {
            var children = await _context.Query.GetChildren<T>(_entity, name);
            return CreateDocuments(children);
        }

        public async Task<IEnumerable<Document<T>>> GetChildren<T>()
        {
            var children = await _context.Query.GetChildren<T>(_entity);
            return CreateDocuments(children);
        }

        public async Task<IEnumerable<Document<T>>> GetChildren<T>(Expression<Func<T, bool>> filter)
        {
            var children = await _context.Query.GetChildren(_entity, filter);
            return CreateDocuments(children);
        }

        public async Task<IEnumerable<Document<T>>> GetChildren<T>(IEnumerable<Guid> ids)
        {
            var children = await _context.Query.GetChildren<T>(_entity, ids);
            return CreateDocuments(children);
        }

        public async Task<long> GetChildrenCount<T>()
        {
            return await _context.Query.GetChildrenCount<T>(_entity);
        }

        public async Task<long> GetChildrenCount()
        {
            return await _context.Query.GetChildrenCount(_entity);
        }

        #endregion

        #region Subscription

        public IEntitySubscription SubscribeChanges(Action<DocumentResult<TEntity>> callback)
        {
            return Context.Documents.SubscribeDocument(Id, callback);
        }

        public IEntitySubscription SubscribeChanges(Func<DocumentResult<TEntity>, Task> callback)
        {
            return Context.Documents.SubscribeDocument(Id, callback);
        }

        #endregion

        #region Comparsions

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Document<TEntity>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            var item = (Document<TEntity>)obj;

            return item.Id.Equals(this.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Document<TEntity> left, Document<TEntity> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Document<TEntity> left, Document<TEntity> right)
        {
            return !(left == right);
        }

        #endregion

        #region Conversions

        private void ChekId<T>(T item)
        {
            var itemId = GetId(item);
            if (Id != itemId)
                throw new InvalidOperationException($"Mismatching entity id. Expected {Id} - actual value {itemId}");
        }

        private Guid GetId<T>(T item)
        {
            return _context.Resolver.GetEntityId(item);
        }

        private IEnumerable<Document<T>> CreateDocuments<T>(IEnumerable<T> values)
        {
            return values
                .Select(x => CreateDocument(x))
                .ToList();
        }

        private Document<T> CreateDocument<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return new Document<T>(_context, value);
        }

        #endregion
    }

    public class DocumentContainer : Document<Container>
    {
        internal DocumentContainer(EntityContext context, Container entity) : base(context, entity)
        {
        }
    }
}
