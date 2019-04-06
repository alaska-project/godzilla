using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Utils
{
    public static class BsonUtil
    {
        public static string SerializeToJson(BsonDocument document)
        {
            var settings = new JsonWriterSettings
            {
                OutputMode = JsonOutputMode.Shell
            };
            var jsonValue = document.ToJson(settings);
            return jsonValue;
        }
    }
}
