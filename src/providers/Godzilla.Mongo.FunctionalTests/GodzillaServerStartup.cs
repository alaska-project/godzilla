using Godzilla.DemoWebApp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Mongo.FunctionalTests
{
    public class GodzillaServerStartup : Startup
    {
        public GodzillaServerStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
