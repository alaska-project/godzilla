using Godzilla.Abstractions.Infrastructure;
using Godzilla.Mongo.Utils;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Mongo.Infrastructure
{
    internal class MongoDatabaseRawCollection : IDatabaseCollection
    {
        private const string IdField = "_id";
        private readonly IMongoCollection<BsonDocument> _collection;

        public MongoDatabaseRawCollection(
            IMongoCollection<BsonDocument> collection,
            string collectionId)
        {
            _collection = collection;
            CollectionId = collectionId;
        }

        public string CollectionId { get; }

        public async Task<string> GetRawItem(Guid id)
        {
            var document = await _collection
                .Find(GetEntityIdFilter(id))
                .FirstOrDefaultAsync();

            return BsonUtil.SerializeToJson(document);
        }

        private FilterDefinition<BsonDocument> GetEntityIdFilter(Guid entityId)
        {
            return Builders<BsonDocument>.Filter.Eq(IdField, entityId);
        }
    }
}
