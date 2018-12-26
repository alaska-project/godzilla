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
                var item = await context.Commands.Add(new TestEntity
                {
                    Name = "gigi"
                });

                Assert.NotEqual(Guid.Empty, item.Id);

                var foundItem = context.Query
                    .AsQueryable<TestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);
            }
        }

        [Fact]
        public async Task Create_derived_item()
        {
            using (var server = CreateServer())
            {
                var context = GetEntityContext<TestEntityContext>(server);
                var item = await context.Commands.Add(new DerivedTestEntity
                {
                    Name = "gigi"
                });

                Assert.NotEqual(Guid.Empty, item.Id);
                var foundItem = context.Query
                    .AsQueryable<DerivedTestEntity>()
                    .FirstOrDefault(x => x.Id == item.Id);

                Assert.NotNull(foundItem);
                Assert.Equal(item.Id, foundItem.Id);
                Assert.Equal(item.Name, foundItem.Name);
            }
        }
    }
}
