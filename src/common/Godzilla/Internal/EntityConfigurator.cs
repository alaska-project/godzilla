using Godzilla.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Internal
{
    internal class EntityConfigurator<TContext> : IEntityConfigurator
        where TContext : EntityContext
    {
        private readonly ICollectionService<TContext> _collectionService;

        public EntityConfigurator(ICollectionService<TContext> collectionService)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
        }

        public async Task DefineIndex<TEntity>(IndexDefinition<TEntity> index)
        {
            var collection = _collectionService.GetCollection<TEntity>();
            await collection.CreateIndex(index);
        }
    }
}
