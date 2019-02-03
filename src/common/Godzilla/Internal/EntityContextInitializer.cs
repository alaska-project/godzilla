﻿using Godzilla.Abstractions.Services;
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
            configurator.DefineIndex(new IndexDefinition<EntityNode>
            {
                Name = "path_asc",
                Fields = new List<IndexField<EntityNode>>
                {
                    new IndexField<EntityNode>(x => x.Reference.Path, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<EntityNode>
            {
                Name = "parentId_asc",
                Fields = new List<IndexField<EntityNode>>
                {
                    new IndexField<EntityNode>(x => x.Reference.ParentId, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<EntityNode>
            {
                Name = "idPath_asc",
                Fields = new List<IndexField<EntityNode>>
                {
                    new IndexField<EntityNode>(x => x.Reference.IdPath, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<EntityNode>
            {
                Name = "nodeId_asc",
                Fields = new List<IndexField<EntityNode>>
                {
                    new IndexField<EntityNode>(x => x.Reference.NodeId, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = true,
                }
            });

            configurator.DefineIndex(new IndexDefinition<Container>
            {
                Name = "containerName_asc",
                Fields = new List<IndexField<Container>>
                {
                    new IndexField<Container>(x => x.Name, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });
        }
    }
}
