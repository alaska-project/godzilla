using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Godzilla.Mongo.FunctionalTests
{
    public abstract class MongoGodzillaScenarioBase
    {
        protected TContext GetEntityContext<TContext>(TestServer server)
            where TContext : EntityContext
        {
            return server.Host.Services.GetRequiredService<TContext>();
        }

        protected TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(MongoGodzillaScenarioBase))
                .Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                    .UseStartup<GodzillaApiServerStartup>();

            var testServer = new TestServer(hostBuilder);
            return testServer;
        }
    }
}
