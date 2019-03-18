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
        public async Task ManageDocuments()
        {
            using (var server = CreateServer())
            {
                #region Init

                var context = GetEntityContext<TestEntityContext>(server);

                var root = await context.Documents.CreateDocument(new TestEntity
                {
                    Name = "doc-root"
                });

                #endregion

                #region Subscribe Document

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

                #endregion
                
                #region Subscibe Document with initial value

                callbacks.Clear();

                Assert.Empty(callbacks);

                using (context.Documents.SubscribeDocument(root.Id, callback, true))
                {
                    await root.UpdateField(x => x.Val2, "val2");
                    await root.UpdateField(x => x.Val2, "val3");

                    Thread.Sleep(1000);
                }

                Assert.Equal(3, callbacks.Count);

                #endregion

                #region Add Child

                await root.AddChild("child", new TestEntity { Val2 = "vattelappesca" });

                var child = await root.GetChild<TestEntity>("child");

                Assert.NotNull(child);

                #endregion

                #region Children Count

                await root.AddChild("child2", new TestEntity { Val2 = "vattelappesca" });
                await root.AddChild("child3", new TestEntity { Val2 = "vattelappesca" });

                var count = await root.GetChildrenCount();
                Assert.Equal(3, count);

                count = await root.GetChildrenCount<TestEntity>();
                Assert.Equal(3, count);

                #endregion

                await root.Delete();
            }
        }
    }
}
