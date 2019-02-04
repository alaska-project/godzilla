using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.FunctionalTests
{
    public class GodzillaAuthenticatedApiServerStartup : GodzillaApiServerStartup
    {
        public GodzillaAuthenticatedApiServerStartup(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddEntityContext<TestEntityContext>(opt =>
                {
                    opt.UseMongoDb<TestEntityContext>(
                            _runner.ConnectionString,
                            Configuration["Godzilla:Database"]);
                })
                .AddEntityContextAuthorization();
        }
    }
}
