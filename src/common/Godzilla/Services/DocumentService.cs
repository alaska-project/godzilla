using Godzilla.Abstractions;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Services
{
    internal class DocumentService : IDocumentService
    {
        #region Init

        private readonly EntityContext _context;
        private readonly IEntityQueries _queries;
        private readonly IEntityCommands _commands;

        public DocumentService(
            EntityContext context,
            IEntityQueries queries,
            IEntityCommands commands)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        #endregion

        #region Documents Get

        public async Task<DocumentContainer> Container(string name)
        {
            var container = await _queries.GetItem<Container>(name);
            if (container == null)
                container = await _commands.Add(new Container
                {
                    Name = name
                });

            return new DocumentContainer(_context, container);
        }

        public async Task<Document<TItem>> CreateDocument<TItem>(TItem item)
        {
            var newItem = await _commands.Add(item);
            return NewDocument(newItem);
        }

        public async Task<Document<TItem>> GetDocument<TItem>(Guid id)
        {
            var item = await _queries.GetItem<TItem>(id);
            return NewDocument(item);
        }

        public async Task<Document<TItem>> GetDocument<TItem>(string path)
        {
            var item = await _queries.GetItem<TItem>(path);
            return NewDocument(item);
        }

        public async Task<Document<TItem>> GetDocument<TItem>(Expression<Func<TItem, bool>> filter)
        {
            var item = await _queries.GetItem<TItem>(filter);
            return NewDocument(item);
        }

        public async Task<IEnumerable<Document<TItem>>> GetDocuments<TItem>(string path)
        {
            var items = await _queries.GetItems<TItem>(path);
            return NewDocuments(items);
        }

        public async Task<IEnumerable<Document<TItem>>> GetDocuments<TItem>(IEnumerable<Guid> id)
        {
            var items = await _queries.GetItems<TItem>(id);
            return NewDocuments(items);
        }

        public async Task<IEnumerable<Document<TItem>>> GetDocuments<TItem>(Expression<Func<TItem, bool>> filter)
        {
            var items = await _queries.GetItems<TItem>(filter);
            return NewDocuments(items);
        }

        #endregion

        #region Document Subscribe

        public IDisposable SubscribeDocument<TItem>(Guid entityId, Action<DocumentResult<TItem>> callback)
        {
            return _context.NotificationService.SubscribeEntityEvent(entityId, async () =>
            {
                var documentResult = await CreateDocumentResult<TItem>(entityId);
                callback.Invoke(documentResult);
            });
        }

        public IDisposable SubscribeDocument<TItem>(Guid entityId, Func<DocumentResult<TItem>, Task> callback)
        {
            return _context.NotificationService.SubscribeEntityEvent(entityId, async () => 
            {
                var documentResult = await CreateDocumentResult<TItem>(entityId);
                await callback.Invoke(documentResult);
            });
        }

        private async Task<DocumentResult<TItem>> CreateDocumentResult<TItem>(Guid entityId)
        {
            var document = await GetDocument<TItem>(entityId);
            return new DocumentResult<TItem>(document != null, document);
        }
        
        #endregion

        #region Conversions

        private IEnumerable<Document<T>> NewDocuments<T>(IEnumerable<T> values)
        {
            return values
                .Select(x => NewDocument(x))
                .Where(x => x != null)
                .ToList();
        }

        private Document<T> NewDocument<T>(T value)
        {
            return value == null ?
                null :
                new Document<T>(_context, value);
        }
        
        #endregion
    }
}
