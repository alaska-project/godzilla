using Godzilla.Abstractions.Infrastructure;
using Godzilla.Events.Providers;
using Godzilla.Events.Queue;
using Godzilla.Mongo.EventQueue;
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
    public enum EventQueueType { InMemory, Mongo }

    public static class MongoEntityContextOptionsBuilderExtensions
    {
        public static void UseMongoDb<TContext>(this EntityContextOptionsBuilder<TContext> builder, 
            string connectionString, 
            string database,
            EventQueueType eventQueueType = EventQueueType.InMemory)
            where TContext : EntityContext
        {
            RegisterConventions(MongoDB.Bson.GuidRepresentation.Standard);

            var options = new MongoEntityContextOptions<TContext>
            {
                ConnectionString = connectionString,
                DatabaseName = database,
            };
            builder.Builder
                .Services
                .AddSingleton(options)
                .AddTransient<IDatabaseTransactionManager<TContext>, MongoDatabaseTransactionManager<TContext>>()
                .AddScoped<IDatabaseCollectionProvider<TContext>, MongoDatabaseCollectionProvider<TContext>>()
                .AddScoped<MongoDatabaseFactory<TContext>>()
                .AddEventQueue<TContext>(eventQueueType);
        }

        private static IServiceCollection AddEventQueue<TContext>(this IServiceCollection services, EventQueueType eventQueueType)
            where TContext : EntityContext
        {
            switch (eventQueueType)
            {
                case EventQueueType.InMemory:
                    return services.AddInMemoryEventQueue<TContext>();
                case EventQueueType.Mongo:
                    return services.AddMongoEventQueue<TContext>();
                default:
                    throw new NotImplementedException($"Event queue {eventQueueType} not implemented");
            }
        }

        private static IServiceCollection AddInMemoryEventQueue<TContext>(this IServiceCollection services)
            where TContext : EntityContext
        {
            return services
                .AddSingleton<IEventQueueProvider<TContext>, InMemoryEventQueueProvider<TContext>>()
                .AddSingleton<IEventQueue<TContext>, DefaultEventQueue<TContext>>();
        }

        private static IServiceCollection AddMongoEventQueue<TContext>(this IServiceCollection services)
            where TContext : EntityContext
        {
            return services
                .AddSingleton<IEventQueueProvider<TContext>, MongoEventQueueProvider<TContext>>()
                .AddSingleton<IEventQueue<TContext>, DefaultEventQueue<TContext>>();
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
