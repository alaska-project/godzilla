using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security.Models
{
    internal class EntityNodeRules
    {
        public EntityNodeRules(EntityNode node, IEnumerable<EntityNodeRuleContainer> rules)
        {
            Node = node;
            Rules = rules;
        }

        public EntityNode Node { get; }
        public IEnumerable<EntityNodeRuleContainer> Rules { get; }
    }

    internal class EntityNodeRuleContainer
    {
        public EntityNodeRuleContainer(EntityNode ruleNode, EntitySecurityRule securityRule)
        {
            SecurityRule = securityRule;
            NodeIdPath = ruleNode.IdPath.Substring(
                0,
                ruleNode.IdPath.IndexOf(ruleNode.EntityId.ToString()) + ruleNode.EntityId.ToString().Length
                );
        }

        public string NodeIdPath { get; }
        public EntitySecurityRule SecurityRule { get; }
    }
}
