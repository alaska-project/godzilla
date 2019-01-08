﻿using Microsoft.Extensions.DependencyInjection;
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

                #region AsQueryable retreive

                var foundItem = context.Query
                    .AsQueryable<TestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);
                
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region GetItem retreive

                foundItem = context.Query.GetItem<TestEntity>(item.Id);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                var foundItems = context.Query.GetItems<TestEntity>("/root/gigi");
                Assert.NotEmpty(foundItems);
                Assert.Contains(foundItems, x => x.Id == item.Id);
                Assert.Contains(foundItems, x => x.Name == item.Name);

                #endregion

                #region GetChild retreive

                foundItem = context.Query.GetChild<TestEntity>(rootItem);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region filtered GetChild retreive

                foundItem = context.Query.GetChild<TestEntity>(rootItem, x => x.Name == "gigio");
                Assert.Null(foundItem);

                foundItem = context.Query.GetChild<TestEntity>(rootItem, x => x.Name == "gigi");
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region update

                var itemToUpdate = context.Query.GetItem<TestEntity>(item.Id);
                itemToUpdate.Name = "gigi-new";
                var updatedItem = await context.Commands.Update(itemToUpdate);

                Assert.NotNull(updatedItem);
                Assert.Equal(itemToUpdate.Name, updatedItem.Name);

                #endregion

                #region rename

                await context.Commands.Rename(rootItem, "root-new");

                var renamedRootItems = context.Query.GetItems<TestEntity>("/root-new");
                var renamedRootItem = renamedRootItems.FirstOrDefault(x => x.Id == rootItem.Id);
                Assert.NotNull(renamedRootItem);

                var renamedItems = context.Query.GetItems<TestEntity>("/root-new/gigi");
                var renamedItem = renamedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(renamedItem);

                #endregion

                #region Move

                var rootItem2 = await context.Commands.Add(new TestEntity
                {
                    Name = "root2"
                });

                await context.Commands.Move(rootItem, rootItem2);

                var movedItems = context.Query.GetItems<TestEntity>("/root2/root-new/gigi");
                var movedItem = movedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(movedItem);

                await context.Commands.Move(movedItem, rootItem2);

                var secondMovedItems = context.Query.GetItems<TestEntity>("/root2/gigi");
                var secondMovedItem = secondMovedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(secondMovedItem);

                #endregion

                #region delete

                await context.Commands.Delete(rootItem2);

                var foundItemAfterDelete = context.Query
                    .AsQueryable<TestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.Null(foundItemAfterDelete);

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

                #region AsQueryable retreive

                Assert.NotEqual(Guid.Empty, item.Id);
                var foundItem = context.Query
                    .AsQueryable<DerivedTestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region GetItem retreive

                foundItem = context.Query.GetItem<DerivedTestEntity>(item.Id);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                var foundItems = context.Query.GetItems<TestEntity>("/root/gigi");
                Assert.NotEmpty(foundItems);
                Assert.Contains(foundItems, x => x.Id == item.Id);
                Assert.Contains(foundItems, x => x.Name == item.Name);

                #endregion

                #region GetChild retreive

                foundItem = context.Query.GetChild<DerivedTestEntity>(rootItem);
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region filtered GetChild retreive

                foundItem = context.Query.GetChild<DerivedTestEntity>(rootItem, x => x.Name == "gigio");
                Assert.Null(foundItem);

                foundItem = context.Query.GetChild<DerivedTestEntity>(rootItem, x => x.Name == "gigi");
                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);

                #endregion

                #region Update

                var itemToUpdate = context.Query.GetItem<DerivedTestEntity>(item.Id);
                itemToUpdate.Name = "gigi-new";
                var updatedItem = await context.Commands.Update(itemToUpdate);

                Assert.NotNull(updatedItem);
                Assert.Equal(itemToUpdate.Name, updatedItem.Name);

                #endregion

                #region Rename

                await context.Commands.Rename(rootItem, "root-new");

                var renamedRootItems = context.Query.GetItems<TestEntity>("/root-new");
                var renamedRootItem = renamedRootItems.FirstOrDefault(x => x.Id == rootItem.Id);
                Assert.NotNull(renamedRootItem);

                var renamedItems = context.Query.GetItems<DerivedTestEntity>("/root-new/gigi");
                var renamedItem = renamedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(renamedItem);

                #endregion

                #region Move

                var rootItem2 = await context.Commands.Add(new TestEntity
                {
                    Name = "root2"
                });

                await context.Commands.Move(rootItem, rootItem2);

                var movedItems = context.Query.GetItems<DerivedTestEntity>("/root2/root-new/gigi");
                var movedItem = movedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(movedItem);

                await context.Commands.Move(movedItem, rootItem2);

                var secondMovedItems = context.Query.GetItems<DerivedTestEntity>("/root2/gigi");
                var secondMovedItem = secondMovedItems.FirstOrDefault(x => x.Id == item.Id);
                Assert.NotNull(secondMovedItem);

                #endregion

                #region delete

                await context.Commands.Delete(rootItem2);

                var foundItemAfterDelete = context.Query
                    .AsQueryable<DerivedTestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.Null(foundItemAfterDelete);

                #endregion
            }
        }
    }
}
