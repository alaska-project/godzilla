using Godzilla.Abstractions.Services;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Internal
{
    internal class EntityContextInitializer<TContext> : IEntityContextInitializer
        where TContext : EntityContext
    {
        private bool _isInitialized = false;

        public void Initialize(EntityContext context, IEntityConfigurator configurator)
        {
            lock (this)
            {
                if (_isInitialized)
                    return;

                InitializeCommonIndexes(configurator);

                context.OnConfiguring();

                _isInitialized = true;
            }
        }

        private void InitializeCommonIndexes(IEntityConfigurator configurator)
        {
            configurator.DefineIndex(new IndexDefinition<TreeEdge>
            {
                Name = "path_asc",
                Fields = new List<IndexField<TreeEdge>>
                {
                    new IndexField<TreeEdge>(x => x.Path, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<TreeEdge>
            {
                Name = "parentId_asc",
                Fields = new List<IndexField<TreeEdge>>
                {
                    new IndexField<TreeEdge>(x => x.ParentId, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<TreeEdge>
            {
                Name = "idPath_asc",
                Fields = new List<IndexField<TreeEdge>>
                {
                    new IndexField<TreeEdge>(x => x.IdPath, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<TreeEdge>
            {
                Name = "nodeId_asc",
                Fields = new List<IndexField<TreeEdge>>
                {
                    new IndexField<TreeEdge>(x => x.NodeId, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = true,
                }
            });
        }
    }
}
