using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IDocumentService
    {
        Task<Document<Container>> Container(string name);
        Task<Document<TItem>> CreateDocument<TItem>(TItem item);

        Document<TItem> GetDocument<TItem>(Guid id);
        Document<TItem> GetDocument<TItem>(string path);
        Document<TItem> GetDocument<TItem>(Expression<Func<TItem, bool>> filter);
        IEnumerable<Document<TItem>> GetDocuments<TItem>(string path);
        IEnumerable<Document<TItem>> GetDocuments<TItem>(IEnumerable<Guid> id);
        IEnumerable<Document<TItem>> GetDocuments<TItem>(Expression<Func<TItem, bool>> filter);
    }
}
