using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Godzilla.Abstractions.Infrastructure
{
    public enum IndexSortOrder { Asc, Desc }

    public interface IIndexField<T>
    {
        Expression<Func<T, object>> Field { get; }
        IndexSortOrder SortOrder { get; }
    }
}
