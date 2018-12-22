using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Infrastructure
{
    //TODO
    internal class MongoDiscriminatorConvention : IDiscriminatorConvention
    {
        public string ElementName => "_t";

        public Type GetActualType(IBsonReader bsonReader, Type nominalType)
        {
            throw new NotImplementedException();
        }

        public BsonValue GetDiscriminator(Type nominalType, Type actualType)
        {
            throw new NotImplementedException();
        }
    }
}
