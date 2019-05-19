using Godzilla.Abstractions;
using Godzilla.DomainModels;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger _logger;
        private readonly EntityContext _context;
        private readonly IEntityQueries _queries;
        private readonly IEntityCommands _commands;
        private readonly ISecurityDisablerService _securityDisabler;

        public DocumentService(
            ILogger logger,
            EntityContext context,
            IEntityQueries queries,
            IEntityCommands commands,
            ISecurityDisablerService securityDisabler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _securityDisabler = securityDisabler;
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

        public IEntitySubscription SubscribeDocument<TItem>(Guid entityId, Action<DocumentResult<TItem>> callback)
        {
            return SubscribeDocument(entityId, callback, false);
        }

        public IEntitySubscription SubscribeDocument<TItem>(Guid entityId, Action<DocumentResult<TItem>> callback, bool getInitialValue)
        {
            if (getInitialValue)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var documentResult = await CreateDocumentResult<TItem>(entityId);
                        callback.Invoke(documentResult);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Error invoking document subscription callback for entity {entityId}");
                    }
                })
                .ConfigureAwait(false);
            }

            return _context.NotificationService.SubscribeEntityEvent(entityId, async () =>
            {
                var documentResult = await CreateDocumentResult<TItem>(entityId);
                callback.Invoke(documentResult);
            });
        }

        public IEntitySubscription SubscribeDocument<TItem>(Guid entityId, Func<DocumentResult<TItem>, Task> callback)
        {
            return SubscribeDocument(entityId, callback, false);
        }

        public IEntitySubscription SubscribeDocument<TItem>(Guid entityId, Func<DocumentResult<TItem>, Task> callback, bool getInitialValue)
        {
            if (getInitialValue)
            {
                Task.Run(async () =>
                {

                    try
                    {
                        var documentResult = await CreateDocumentResult<TItem>(entityId);
                        await callback.Invoke(documentResult);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Error invoking document subscription callback for entity {entityId}");
                    }
                })
                .ConfigureAwait(false);
            }

            return _context.NotificationService.SubscribeEntityEvent(entityId, async () =>
            {
                var documentResult = await CreateDocumentResult<TItem>(entityId);
                await callback.Invoke(documentResult);
            });
        }

        private async Task<DocumentResult<TItem>> CreateDocumentResult<TItem>(Guid entityId)
        {
            using (_securityDisabler.DisableSecurity())
            {
                var document = await GetDocument<TItem>(entityId);
                return new DocumentResult<TItem>(document != null, document);
            }
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
