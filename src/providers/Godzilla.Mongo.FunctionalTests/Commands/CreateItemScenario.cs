using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
                var item = await context.Add(new TestEntity
                {
                    Name = "gigi"
                });

                Assert.NotEqual(Guid.Empty, item.Id);
            }
        }

        [Fact]
        public async Task Create_derived_item()
        {
            using (var server = CreateServer())
            {
                var context = GetEntityContext<TestEntityContext>(server);
                var item = await context.Add(new DerivedTestEntity
                {
                    Name = "gigi"
                });

                Assert.NotEqual(Guid.Empty, item.Id);
            }
        }
    }
}
