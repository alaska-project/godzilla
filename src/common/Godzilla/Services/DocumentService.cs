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

        public async Task<Document<Container>> Container(string name)
        {
            var container = GetDocument<Container>(name);
            if (container == null)
                container = await CreateDocument(new Container
                {
                    Name = name
                });

            return container;
        }

        public async Task<Document<TItem>> CreateDocument<TItem>(TItem item)
        {
            var newItem = await _commands.Add(item);
            return NewDocument(newItem);
        }

        public Document<TItem> GetDocument<TItem>(Guid id)
        {
            var item = _queries.GetItem<TItem>(id);
            return NewDocument(item);
        }

        public Document<TItem> GetDocument<TItem>(string path)
        {
            var item = _queries.GetItem<TItem>(path);
            return NewDocument(item);
        }

        public Document<TItem> GetDocument<TItem>(Expression<Func<TItem, bool>> filter)
        {
            var item = _queries.GetItem<TItem>(filter);
            return NewDocument(item);
        }

        public IEnumerable<Document<TItem>> GetDocuments<TItem>(string path)
        {
            var items = _queries.GetItems<TItem>(path);
            return NewDocuments(items);
        }

        public IEnumerable<Document<TItem>> GetDocuments<TItem>(IEnumerable<Guid> id)
        {
            var items = _queries.GetItems<TItem>(id);
            return NewDocuments(items);
        }

        public IEnumerable<Document<TItem>> GetDocuments<TItem>(Expression<Func<TItem, bool>> filter)
        {
            var items = _queries.GetItems<TItem>(filter);
            return NewDocuments(items);
        }

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
