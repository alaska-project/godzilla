using Godzilla.Abstractions.Infrastructure;
using Godzilla.Mongo.Infrastructure;
using Godzilla.Mongo.Services;
using Godzilla.Mongo.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo
{
    public static class MongoEntityContextOptionsBuilderExtensions
    {
        public static void UseMongoDb<TContext>(this EntityContextOptionsBuilder builder, 
            string connectionString, 
            string database)
            where TContext : EntityContext
        {
            RegisterConventions(MongoDB.Bson.GuidRepresentation.Standard);

            var options = new MongoEntityContextOptions<TContext>
            {
                ConnectionString = connectionString,
                DatabaseName = database,
            };
            builder.Services
                .AddSingleton(options)
                .AddTransient<IDatabaseTransactionManager<TContext>, MongoDatabaseTransactionManager<TContext>>()
                .AddScoped<IDatabaseCollectionProvider<TContext>, MongoDatabaseCollectionProvider<TContext>>()
                .AddScoped<MongoDatabaseFactory<TContext>>();
        }

        private static void RegisterConventions(MongoDB.Bson.GuidRepresentation? guidRepresentation)
        {
            if (guidRepresentation.HasValue)
                MongoDefaults.GuidRepresentation = guidRepresentation.Value;

            var conventions = new ConventionPack
            {
                //new GuidAsStringRepresentationConvention(new List<System.Reflection.Assembly>()),
                new IgnoreExtraElementsConvention(true)
            };
            ConventionRegistry.Register("customConventions", conventions, x => true);
        }
    }
}
