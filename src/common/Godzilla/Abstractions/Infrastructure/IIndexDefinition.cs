using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IIndexDefinition<TItem>
    {
        string Name { get; }
        List<IndexField<TItem>> Fields { get; }
        IndexOptions Options { get; }
    }
}
