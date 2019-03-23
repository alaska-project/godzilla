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
                #region Init

                var context = GetEntityContext<TestEntityContext>(server);
                var root = await context.Documents.CreateDocument(new TestEntity
                {
                    Name = "ui-doc-root"
                });

                #endregion

                var contexts = await client.GetJsonAsync<List<UiEntityContextReference>>($"{UiManagementApi}/GetContexts");

                Assert.Single(contexts);

                var contextId = contexts.First().Id;

                var rootNodes = await client.GetJsonAsync<List<UiNodeReference>>($"{UiManagementApi}/GetRootNodes?contextId={contextId}");

                Assert.NotEmpty(rootNodes);

                var uiRootNode = rootNodes
                    .FirstOrDefault(x => x.Id == root.Id);

                Assert.NotNull(uiRootNode);

                var rootNodeValue = await client.GetJsonAsync<UiNodeValue>($"{UiManagementApi}/GetNode?contextId={contextId}&nodeId={uiRootNode.Id}");

                var childNodes = await client.GetJsonAsync<List<UiEntityContextReference>>($"{UiManagementApi}/GetChildNodes?contextId={contextId}&parentId={uiRootNode.Id}");

                await root.Delete();
            }
        }
    }
}
