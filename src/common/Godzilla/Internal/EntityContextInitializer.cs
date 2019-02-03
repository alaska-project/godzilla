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
                    new IndexField<EntityNode>(x => x.Reference.EntityId, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = true,
                }
            });

            configurator.DefineIndex(new IndexDefinition<EntitySecurityRule>
            {
                Name = "entityId_asc",
                Fields = new List<IndexField<EntitySecurityRule>>
                {
                    new IndexField<EntitySecurityRule>(x => x.EntityId, Abstractions.Infrastructure.IndexSortOrder.Asc)
                },
                Options = new IndexOptions
                {
                    Unique = false,
                }
            });

            configurator.DefineIndex(new IndexDefinition<EntitySecurityRule>
            {
                Name = "uniqueSecurityRule_asc",
                Fields = new List<IndexField<EntitySecurityRule>>
                {
                    new IndexField<EntitySecurityRule>(x => x.EntityId, Abstractions.Infrastructure.IndexSortOrder.Asc),
                    new IndexField<EntitySecurityRule>(x => x.Subject.SubjectId, Abstractions.Infrastructure.IndexSortOrder.Asc),
                    new IndexField<EntitySecurityRule>(x => x.Subject.SubjectType, Abstractions.Infrastructure.IndexSortOrder.Asc),
                    new IndexField<EntitySecurityRule>(x => x.Rule.Right, Abstractions.Infrastructure.IndexSortOrder.Asc),
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
