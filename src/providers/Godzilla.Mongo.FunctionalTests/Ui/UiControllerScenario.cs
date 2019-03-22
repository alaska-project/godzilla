using Godzilla.AspNetCore.Ui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Godzilla.Mongo.FunctionalTests.Ui
{
    public class UiControllerScenario : MongoGodzillaScenarioBase
    {
        private const string UiManagementApi = "/godzilla/api/UiManagement";
        [Fact]
        public async Task TestUiApi()
        {
            using (var server = CreateServer())
            using (var client = server.CreateClient())
            {
                var contexts = await client.GetJsonAsync<List<UiEntityContextReference>>($"{UiManagementApi}/GetContexts");

                Assert.Single(contexts);

                var contextId = contexts.First().Id;

                var rootNodes = await client.GetJsonAsync<List<UiEntityContextReference>>($"{UiManagementApi}/GetRootNodes?contextId={contextId}");

                Assert.NotEmpty(rootNodes);
            }
        }
    }
}
