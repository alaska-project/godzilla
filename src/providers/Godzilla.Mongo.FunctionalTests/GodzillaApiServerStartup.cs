using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.FunctionalTests
{
    public class GodzillaApiServerStartup
    {
        protected MongoDbRunner _runner;

        public GodzillaApiServerStartup(IConfiguration configuration)
        {
            Configuration = configuration;

            _runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: true);
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddEntityContext<TestEntityContext>(opt =>
                {
                    opt.UseMongoDb<TestEntityContext>(
                            _runner.ConnectionString, 
                            Configuration["Godzilla:Database"]);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
        }
    }
}
