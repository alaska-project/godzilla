using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public class DocumentResult<TEntity>
    {
        public DocumentResult(bool documentExists, Document<TEntity> document)
        {
            DocumentExists = documentExists;
            Document = document;
        }

        public bool DocumentExists { get; }
        public Document<TEntity> Document { get; }
    }
}
