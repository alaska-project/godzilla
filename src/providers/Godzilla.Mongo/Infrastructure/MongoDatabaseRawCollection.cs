using Godzilla.Abstractions.Infrastructure;
using MongoDB.Bson;
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

        public Task<string> GetRawItem(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
