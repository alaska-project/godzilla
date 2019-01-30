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
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entity = entity;
            Id = context.Resolver.GetEntityId(entity);
        }

        public Guid Id { get; }
        public TEntity Value => _entity;

        #endregion

        #region Commands

        public async void Delete()
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
            ChekId(_entity);

            var entity = await _context.Commands.Add(child, _entity);
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
            var container = _context.Query.GetChild<Container>(_entity, x => x.Name.ToLower() == name.ToLower());
            if (container == null)
                container = await _context.Commands.Add(new Container
                {
                    Name = name
                }, _entity);

            return new DocumentContainer(_context, container);
        }

        #endregion

        #region Query

        public Document<T> GetParent<T>()
        {
            var parent = _context.Query.GetParent<T>(_entity);

            return CreateDocument(parent);
        }

        public Document<T> GetChild<T>()
        {
            var children = GetChildren<T>();
            return children.FirstOrDefault();
        }

        public Document<T> GetChild<T>(Expression<Func<T, bool>> filter)
        {
            var children = GetChildren<T>(filter);
            return children.FirstOrDefault();
        }

        public IEnumerable<Document<T>> GetChildren<T>()
        {
            var children = _context.Query.GetChildren<T>(_entity);
            return CreateDocuments(children);
        }

        public IEnumerable<Document<T>> GetChildren<T>(Expression<Func<T, bool>> filter)
        {
            var children = _context.Query.GetChildren(_entity, filter);
            return CreateDocuments(children);
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
