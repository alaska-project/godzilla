using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public class IndexDefinition<TItem> : IIndexDefinition<TItem>
    {
        public string Name { get; set; }
        public List<IndexField<TItem>> Fields { get; set; }
        public IndexOptions Options { get; set; }
    }
}
