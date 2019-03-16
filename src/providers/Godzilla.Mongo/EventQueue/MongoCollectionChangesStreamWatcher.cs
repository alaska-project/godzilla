using Godzilla.Mongo.Abstractions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.EventQueue
{
    internal class MongoCollectionChangesStreamWatcher : MongoCollectionWatcherBase<EventRecord>
    {
        public MongoCollectionChangesStreamWatcher(IMongoCollection<EventRecord> collection) : base(collection)
        {
        }

        public override void Watch(Action<EventRecord> callback)
        {
            var options = new ChangeStreamOptions()
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup
            };

            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<EventRecord>>().Match("{ operationType: { $in: [ 'insert' ] } }");

            var changesCursor = Collection
                .Watch(pipeline, options)
                .ToEnumerable()
                .GetEnumerator();

            while (changesCursor.MoveNext())
            {
                var record = changesCursor.Current.FullDocument;
                InvokeCallback(callback, record);
            }
        }
    }
}
