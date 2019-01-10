using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Godzilla
{
    public class IndexField<T> : IIndexField<T>
    {
        public IndexField(Expression<Func<T, object>> field, IndexSortOrder sortOrder)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public Expression<Func<T, object>> Field { get; }

        public IndexSortOrder SortOrder { get; }
    }
}
