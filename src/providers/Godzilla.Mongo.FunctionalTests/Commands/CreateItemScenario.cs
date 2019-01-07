using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Godzilla.Mongo.FunctionalTests.Commands
{
    public class CreateItemScenario : MongoGodzillaScenarioBase
    {
        [Fact]
        public async Task Create_item()
        {
            using (var server = CreateServer())
            {
                var context = GetEntityContext<TestEntityContext>(server);

                // create root
                var rootItem = await context.Commands.Add(new TestEntity
                {
                    Name = "root"
                });

                // create item
                var item = await context.Commands.Add(new TestEntity
                {
                    Name = "gigi"
                }, rootItem);

                Assert.NotEqual(Guid.Empty, item.Id);

                // AsQueryable retreive
                var foundItem = context.Query
                    .AsQueryable<TestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                // GetItem retreive

                foundItem = context.Query.GetItem<TestEntity>(item.Id);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                // GetChild retreive

                foundItem = context.Query.GetChild<TestEntity>(rootItem);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                // filtered GetChild retreive

                foundItem = context.Query.GetChild<TestEntity>(rootItem, x => x.Name == "gigi");
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                foundItem = context.Query.GetChild<TestEntity>(rootItem, x => x.Name == "gigio");
                Assert.Null(foundItem);

                // update

                var itemToUpdate = context.Query.GetItem<TestEntity>(item.Id);
                itemToUpdate.Name = "gigi-new";
                var updatedItem = await context.Commands.Update(itemToUpdate);

                Assert.NotNull(updatedItem);
                Assert.Equal(itemToUpdate.Name, updatedItem.Name);

                // delete

                await context.Commands.Delete(rootItem);

                var foundItemAfterDelete = context.Query
                    .AsQueryable<TestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.Null(foundItemAfterDelete);
            }
        }

        [Fact]
        public async Task Create_derived_item()
        {
            using (var server = CreateServer())
            {
                var context = GetEntityContext<TestEntityContext>(server);

                // create root
                var rootItem = await context.Commands.Add(new TestEntity
                {
                    Name = "root"
                });

                // create
                
                var item = await context.Commands.Add(new DerivedTestEntity
                {
                    Name = "gigi"
                }, rootItem);

                // AsQueryable retreive

                Assert.NotEqual(Guid.Empty, item.Id);
                var foundItem = context.Query
                    .AsQueryable<DerivedTestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                // GetItem retreive

                foundItem = context.Query.GetItem<DerivedTestEntity>(item.Id);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                // GetChild retreive

                foundItem = context.Query.GetChild<DerivedTestEntity>(rootItem);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                // filtered GetChild retreive

                foundItem = context.Query.GetChild<DerivedTestEntity>(rootItem, x => x.Name == "gigi");
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                foundItem = context.Query.GetChild<DerivedTestEntity>(rootItem, x => x.Name == "gigio");
                Assert.Null(foundItem);

                // update

                var itemToUpdate = context.Query.GetItem<DerivedTestEntity>(item.Id);
                itemToUpdate.Name = "gigi-new";
                var updatedItem = await context.Commands.Update(itemToUpdate);

                Assert.NotNull(updatedItem);
                Assert.Equal(itemToUpdate.Name, updatedItem.Name);

                // delete

                await context.Commands.Delete(rootItem);

                var foundItemAfterDelete = context.Query
                    .AsQueryable<DerivedTestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.Null(foundItemAfterDelete);
            }
        }
    }
}
