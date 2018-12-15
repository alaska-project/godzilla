//using Godzilla.Abstractions;
//using Godzilla.DomainModels;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Godzilla.Collections
//{
//    internal class EventStoreCollection<TContext>
//        where TContext : EntityContext
//    {
//        private readonly IDatabaseCollection<EventStoreEntry> _collection;

//        public EventStoreCollection(
//            IGodzillaOptions<TContext> options,
//            IDatabaseCollectionProvider<TContext> builder)
//        {
//            if (options == null)
//                throw new ArgumentNullException(nameof(options));

//            if (builder == null)
//                throw new ArgumentNullException(nameof(builder));

//            _collection = builder.GetCollection<EventStoreEntry>(options.TreeEdgesCollectionId);
//        }
//    }
//}
