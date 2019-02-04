using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.DomainModels;
using Godzilla.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Security
{
    internal class SecurityRuleMatcher<TContext> : ISecurityRuleMatcher<TContext>
        where TContext : EntityContext
    {
        private readonly ISecurityOptions<TContext> _securityOptions;

        public SecurityRuleMatcher(ISecurityOptions<TContext> securityOptions)
        {
            _securityOptions = securityOptions ?? throw new ArgumentNullException(nameof(securityOptions));
        }

        public EvaluateResult EvaluateRule(EntityNode node, IEnumerable<EntityNodeRuleContainer> rules, Guid permission)
        {
            var filteredRules = rules
                .Where(x =>
                    x.SecurityRule.Rule.Right == permission &&
                    (
                        x.SecurityRule.EntityId == node.EntityId ||
                        x.SecurityRule.Rule.Inherit
                    ))
                .ToList();

            var ruleToApply = filteredRules
                //order from most specific to the most generic
                .OrderByDescending(x => x.NodeIdPath.Length)
                //deny rule wins over allow rule
                .ThenBy(x => x.SecurityRule.Rule.Type == RuleType.Deny ? 0 : 1)
                .Select(x => x.SecurityRule)
                .FirstOrDefault();

            if (ruleToApply == null)
                ruleToApply = _securityOptions.DefaultSecurityRules
                    .First(x => x.Rule.Right == permission);

            var isGranted = ruleToApply.Rule.Type == RuleType.Allow;
            return new EvaluateResult(node.EntityId, permission, isGranted, ruleToApply);
        }
    }
}
