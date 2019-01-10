using Godzilla.Abstractions.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Godzilla.Mongo.Infrastructure
{
    internal static class MongoIndexHelper
    {
        public static CreateIndexOptions CreateIndexOptions(string name, IIndexOptions options)
        {
            return new CreateIndexOptions
            {
                Name = name,
                Unique = options.Unique,
            };
        }

        public static IndexKeysDefinition<T> CreateIndexDefinition<T>(IEnumerable<IIndexField<T>> fields, IIndexOptions options)
        {
            if (fields.Count() < 1)
                throw new InvalidOperationException("Fields length must be greater than 0");

            var firstField = fields.First();
            var otherFields = fields.Skip(1);

            var builder = new IndexKeysDefinitionBuilder<T>();
            var index = CreateIndexDefinition(builder, firstField);
            foreach (var otherField in otherFields)
                index = CreateIndexDefinition(index, otherField);

            return index;
        }

        private static IndexKeysDefinition<T> CreateIndexDefinition<T>(IndexKeysDefinitionBuilder<T> builder, IIndexField<T> field)
        {
            switch (field.SortOrder)
            {
                case IndexSortOrder.Desc:
                    return builder.Descending(field.Field);
                default:
                case IndexSortOrder.Asc:
                    return builder.Ascending(field.Field);
            }
        }

        private static IndexKeysDefinition<T> CreateIndexDefinition<T>(IndexKeysDefinition<T> index, IIndexField<T> field)
        {
            switch (field.SortOrder)
            {
                case IndexSortOrder.Desc:
                    return index.Descending(field.Field);
                default:
                case IndexSortOrder.Asc:
                    return index.Ascending(field.Field);
            }
        }
    }
}
