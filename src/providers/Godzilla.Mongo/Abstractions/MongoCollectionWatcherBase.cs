using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Mongo.Abstractions
{
    internal abstract class MongoCollectionWatcherBase<TElement>
    {
        public MongoCollectionWatcherBase(IMongoCollection<TElement> collection)
        {
            Collection = collection;
        }

        public IMongoCollection<TElement> Collection { get; }

        public abstract void Watch(Action<TElement> callback);

        protected virtual void InvokeCallback(Action<TElement> callback, TElement data)
        {
            Task.Run(() => callback(data))
                .ContinueWith(x =>
                {
                    //TODO: if (x.IsFaulted) -> log
                });
        }
    }
}
