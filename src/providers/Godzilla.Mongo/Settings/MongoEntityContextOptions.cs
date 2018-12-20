using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.Settings
{
    internal class MongoEntityContextOptions<TContext>
        where TContext : EntityContext
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
