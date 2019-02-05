using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
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
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services
                .AddGodzilla()
                .AddEntityContext<TestEntityContext>(opt =>
                {
                    opt.UseMongoDb(
                            _runner.ConnectionString,
                            Configuration["Godzilla:Database"]);
                    opt.UseAuthorization();
                })
                .AddMvcAuthentication();
        }
    }
}
