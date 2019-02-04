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

                #region create root

                var rootItem = await context.Commands.Add(new TestEntity
                {
                    Name = "root"
                });

                #endregion

                #region create item

                var item = await context.Commands.Add(new TestEntity
                {
                    Name = "gigi"
                }, rootItem);

                Assert.NotEqual(Guid.Empty, item.Id);

                #endregion

                //#region AsQueryable retreive

                //var foundItem = context.Query
                //    .AsQueryable<TestEntity>()
                //    .FirstOrDefault(x => x.Id == item.Id);
                
                //Assert.NotNull(foundItem);
                //Assert.Equal(item.Id, foundItem.Id);
                //Assert.Equal(item.Name, foundItem.Name);

                //#endregion

                #region GetItem retreive

                var foundItem = await context.Query.GetItem<TestEntity>(item.Id);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                var foundItems = await context.Query.GetItems<TestEntity>("/root/gigi");
                Assert.NotEmpty(foundItems);
                Assert.Contains(foundItems, x => x.Id == item.Id);
                Assert.Contains(foundItems, x => x.Name == item.Name);

                #endregion

                #region GetChild retreive

                foundItem = await context.Query.GetChild<TestEntity>(rootItem);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region filtered GetChild retreive

                foundItem = await context.Query.GetChild<TestEntity>(rootItem, x => x.Name == "gigio");
                Assert.Null(foundItem);

                foundItem = await context.Query.GetChild<TestEntity>(rootItem, x => x.Name == "gigi");
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region update

                var itemToUpdate = await context.Query.GetItem<TestEntity>(item.Id);
                itemToUpdate.Name = "gigi-new";
                var updatedItem = await context.Commands.Update(itemToUpdate);

                Assert.NotNull(updatedItem);
                Assert.Equal(itemToUpdate.Name, updatedItem.Name);

                #endregion

                #region partial update

                var partialUpdatedItem = await context.Commands.Update<TestEntity, string>(itemToUpdate.Id, x => x.Val2, "val-2");
                Assert.NotNull(partialUpdatedItem);
                Assert.Equal("val-2", partialUpdatedItem.Val2);

                var partialUpdatedItem2 = await context.Commands.Update<TestEntity, string>(itemToUpdate.Id, x => x.Val2, null);
                Assert.NotNull(partialUpdatedItem2);
                Assert.Null(partialUpdatedItem2.Val2);

                #endregion

                #region rename

                await context.Commands.Rename(rootItem, "root-new");

                var renamedRootItems = await context.Query.GetItems<TestEntity>("/root-new");
                var renamedRootItem = renamedRootItems.FirstOrDefault(x => x.Id == rootItem.Id);
                Assert.NotNull(renamedRootItem);

                var renamedItems = await context.Query.GetItems<TestEntity>("/root-new/gigi");
                var renamedItem = renamedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(renamedItem);

                #endregion

                #region Move

                var rootItem2 = await context.Commands.Add(new TestEntity
                {
                    Name = "root2"
                });

                await context.Commands.Move(rootItem, rootItem2);

                var movedItems = await context.Query.GetItems<TestEntity>("/root2/root-new/gigi");
                var movedItem = movedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(movedItem);

                await context.Commands.Move(movedItem, rootItem2);

                var secondMovedItems = await context.Query.GetItems<TestEntity>("/root2/gigi");
                var secondMovedItem = secondMovedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(secondMovedItem);

                #endregion

                #region delete

                await context.Commands.Delete(rootItem2);

                var foundItemAfterDelete = await context.Query
                    .GetItem<TestEntity>(item.Id);

                Assert.Null(foundItemAfterDelete);

                #endregion

                #region filtered delete

                await context.Commands.Add(new TestEntity()
                {
                    Name = "entity-to-delete"
                });

                Assert.NotNull(await context.Query.GetItem<TestEntity>(x => x.Name == "entity-to-delete"));

                await context.Commands.Delete<TestEntity>(x => x.Name == "entity-to-delete");

                Assert.Null(await context.Query.GetItem<TestEntity>(x => x.Name == "entity-to-delete"));

                #endregion
            }
        }

        [Fact]
        public async Task Create_derived_item()
        {
            using (var server = CreateServer())
            {
                var context = GetEntityContext<TestEntityContext>(server);

                #region create root

                var rootItem = await context.Commands.Add(new TestEntity
                {
                    Name = "root"
                });

                #endregion

                #region create

                var item = await context.Commands.Add(new DerivedTestEntity
                {
                    Name = "gigi"
                }, rootItem);

                #endregion

                //#region AsQueryable retreive

                //Assert.NotEqual(Guid.Empty, item.Id);
                //var foundItem = context.Query
                //    .AsQueryable<DerivedTestEntity>()
                //    .FirstOrDefault(x => x.Id == item.Id);

                //Assert.NotNull(foundItem);
                //Assert.Equal(item.Id, foundItem.Id);
                //Assert.Equal(item.Name, foundItem.Name);

                //#endregion

                #region GetItem retreive

                var foundItem = await context.Query.GetItem<DerivedTestEntity>(item.Id);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                var foundItems = await context.Query.GetItems<TestEntity>("/root/gigi");
                Assert.NotEmpty(foundItems);
                Assert.Contains(foundItems, x => x.Id == item.Id);
                Assert.Contains(foundItems, x => x.Name == item.Name);

                #endregion

                #region GetChild retreive

                foundItem = await context.Query.GetChild<DerivedTestEntity>(rootItem);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region filtered GetChild retreive

                foundItem = await context.Query.GetChild<DerivedTestEntity>(rootItem, x => x.Name == "gigio");
                Assert.Null(foundItem);

                foundItem = await context.Query.GetChild<DerivedTestEntity>(rootItem, x => x.Name == "gigi");
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region Update

                var itemToUpdate = await context.Query.GetItem<DerivedTestEntity>(item.Id);
                itemToUpdate.Name = "gigi-new";
                var updatedItem = await context.Commands.Update(itemToUpdate);

                Assert.NotNull(updatedItem);
                Assert.Equal(itemToUpdate.Name, updatedItem.Name);

                #endregion

                #region partial update

                var partialUpdatedItem = await context.Commands.Update<DerivedTestEntity, string>(itemToUpdate.Id, x => x.Val2, "val-2");
                Assert.NotNull(partialUpdatedItem);
                Assert.Equal("val-2", partialUpdatedItem.Val2);

                var partialUpdatedItem2 = await context.Commands.Update<DerivedTestEntity, string>(itemToUpdate.Id, x => x.Val2, null);
                Assert.NotNull(partialUpdatedItem2);
                Assert.Null(partialUpdatedItem2.Val2);

                #endregion

                #region Rename

                await context.Commands.Rename(rootItem, "root-new");

                var renamedRootItems = await context.Query.GetItems<TestEntity>("/root-new");
                var renamedRootItem = renamedRootItems.FirstOrDefault(x => x.Id == rootItem.Id);
                Assert.NotNull(renamedRootItem);

                var renamedItems = await context.Query.GetItems<DerivedTestEntity>("/root-new/gigi");
                var renamedItem = renamedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(renamedItem);

                #endregion

                #region Move

                var rootItem2 = await context.Commands.Add(new TestEntity
                {
                    Name = "root2"
                });

                await context.Commands.Move(rootItem, rootItem2);

                var movedItems = await context.Query.GetItems<DerivedTestEntity>("/root2/root-new/gigi");
                var movedItem = movedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(movedItem);

                await context.Commands.Move(movedItem, rootItem2);

                var secondMovedItems = await context.Query.GetItems<DerivedTestEntity>("/root2/gigi");
                var secondMovedItem = secondMovedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(secondMovedItem);

                #endregion

                #region delete

                await context.Commands.Delete(rootItem2);

                var foundItemAfterDelete = await context.Query
                    .GetItem<DerivedTestEntity>(item.Id);

                Assert.Null(foundItemAfterDelete);

                #endregion

                #region filtered delete

                await context.Commands.Add(new DerivedTestEntity()
                {
                    Name = "derived-entity-to-delete"
                });

                Assert.NotNull(await context.Query.GetItem<DerivedTestEntity>(x => x.Name == "derived-entity-to-delete"));

                await context.Commands.Delete<DerivedTestEntity>(x => x.Name == "derived-entity-to-delete");

                Assert.Null(await context.Query.GetItem<DerivedTestEntity>(x => x.Name == "derived-entity-to-delete"));

                #endregion
            }
        }
    }
}
