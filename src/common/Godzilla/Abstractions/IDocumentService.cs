﻿using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions
{
    public interface IDocumentService
    {
        Task<DocumentContainer> Container(string name);
        Task<Document<TItem>> CreateDocument<TItem>(TItem item);

        Task<Document<TItem>> GetDocument<TItem>(Guid id);
        Task<Document<TItem>> GetDocument<TItem>(string path);
        Task<Document<TItem>> GetDocument<TItem>(Expression<Func<TItem, bool>> filter);
        Task<IEnumerable<Document<TItem>>> GetDocuments<TItem>(string path);
        Task<IEnumerable<Document<TItem>>> GetDocuments<TItem>(IEnumerable<Guid> id);
        Task<IEnumerable<Document<TItem>>> GetDocuments<TItem>(Expression<Func<TItem, bool>> filter);

        IDisposable SubscribeDocument<TItem>(Guid id, Action<DocumentResult<TItem>> callback);
        IDisposable SubscribeDocument<TItem>(Guid id, Func<DocumentResult<TItem>, Task> callback);
    }
}
