using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Security
{
    internal class SecurityRulesFinder<TContext> : ISecurityRulesFinder<TContext>
        where TContext : EntityContext
    {
        private ICollectionService<TContext> _collectionService;
        private IPathBuilder<TContext> _pathBuilder;

        public SecurityRulesFinder(ICollectionService<TContext> collectionService, IPathBuilder<TContext> pathBuilder)
        {
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
        }

        public async Task<IEnumerable<EntityNodeRules>> GetRules(IEnumerable<Guid> entitiesId, IEnumerable<RuleSubject> subjects, Guid accessRight)
        {
            var nodes = GetEntityNodes(entitiesId);

            var nodesIdAndAncestorsId = GetNodesIdAndAncestorsId(nodes);

            var securityRules = await GetEntitiesSecurityRules(nodesIdAndAncestorsId, subjects, accessRight);

            return nodes
                .Select(x => GetEntityRules(x, securityRules))
                .ToList();
        }

        private EntityNodeRules GetEntityRules(EntityNode node, IEnumerable<EntitySecurityRule> securityRules)
        {
            var matchingRules = securityRules
                .Where(x => node.IdPath.Contains(x.EntityId.ToString()))
                .Select(x => new EntityNodeRuleContainer(node, x))
                .ToList();

            return new EntityNodeRules(node, matchingRules);
        }

        private async Task<IEnumerable<EntitySecurityRule>> GetEntitiesSecurityRules(IEnumerable<Guid> entitiesId, IEnumerable<RuleSubject> subjects, Guid accessRight)
        {
            var rulesCollection = _collectionService.GetCollection<EntitySecurityRule, SecurityRulesCollection>();

            return await rulesCollection.GetRules(entitiesId, subjects, new List<Guid> { accessRight });
        }

        private IEnumerable<Guid> GetNodesIdAndAncestorsId(IEnumerable<EntityNode> entityNodes)
        {
            return entityNodes
                .SelectMany(x => _pathBuilder.GetSegments(x.IdPath))
                .Distinct()
                .Select(x => new Guid(x))
                .ToList();
        }

        private IEnumerable<EntityNode> GetEntityNodes(IEnumerable<Guid> entitiesId)
        {
            var nodesCollection = _collectionService.GetCollection<EntityNode, EntityNodesCollection>();

            return nodesCollection.GetNodes(entitiesId);
        }
    }
}
