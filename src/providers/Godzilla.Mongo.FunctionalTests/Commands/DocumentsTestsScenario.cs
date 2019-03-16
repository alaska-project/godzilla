using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Godzilla.Mongo.FunctionalTests.Commands
{
    public class DocumentsTestsScenario : MongoGodzillaScenarioBase
    {
        [Fact]
        public async Task SubscribeDocument()
        {
            using (var server = CreateServer())
            {
                var context = GetEntityContext<TestEntityContext>(server);

                var root = await context.Documents.CreateDocument(new TestEntity
                {
                    Name = "doc-root"
                });

                var callbacks = new List<DocumentResult<TestEntity>>();
                var callback = new Action<DocumentResult<TestEntity>>(x =>
                {
                    callbacks.Add(x);
                    Console.WriteLine($"Callback invoked -> {JsonConvert.SerializeObject(x)}");
                });

                using (context.Documents.SubscribeDocument(root.Id, callback))
                {
                    await root.UpdateField(x => x.Val2, "val2");
                    await root.UpdateField(x => x.Val2, "val3");

                    Thread.Sleep(500);
                }
                
                Assert.Equal(2, callbacks.Count);

                callbacks.Clear();

                Assert.Empty(callbacks);

                using (context.Documents.SubscribeDocument(root.Id, callback, true))
                {
                    await root.UpdateField(x => x.Val2, "val2");
                    await root.UpdateField(x => x.Val2, "val3");

                    Thread.Sleep(500);
                }

                Assert.Equal(3, callbacks.Count);

                await root.Delete();
            }
        }
    }
}
