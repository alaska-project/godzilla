using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.DomainModels;
using Godzilla.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Security
{
    internal class SecurityRuleEvaluator<TContext> : ISecurityRuleEvaluator<TContext>
        where TContext : EntityContext
    {
        private readonly ISecurityRulesFinder<TContext> _rulesFinder;
        private readonly ISecurityRuleMatcher<TContext> _rulesMatcher;
        private readonly ISecurityContext<TContext> _securityContext;
        private readonly ISecurityOptions<TContext> _securityOptions;

        public SecurityRuleEvaluator(
            ISecurityRulesFinder<TContext> rulesFinder,
            ISecurityRuleMatcher<TContext> rulesMatcher,
            ISecurityContext<TContext> securityContext,
            ISecurityOptions<TContext> securityOptions)
        {
            _rulesFinder = rulesFinder ?? throw new ArgumentNullException(nameof(rulesFinder));
            _rulesMatcher = rulesMatcher ?? throw new ArgumentNullException(nameof(rulesMatcher));
            _securityContext = securityContext ?? throw new ArgumentNullException(nameof(securityContext));
            _securityOptions = securityOptions;
        }

        private bool UseAuthorization => _securityOptions?.UseAuthorization ?? false;

        public async Task<IEnumerable<EvaluateResult>> Evaluate(IEnumerable<Guid> entitiesId, Guid permission)
        {
            if (!UseAuthorization ||
                _securityContext.IsAdministrator())
                return entitiesId
                    .Select(x => new EvaluateResult(x, permission, true));

            var appliableSubjects = _securityContext.GetApplyableSubjects();

            var rules = await _rulesFinder.GetRules(entitiesId, appliableSubjects, permission);

            return rules
                .Select(x => EvaluateRule(x.Node, x.Rules, permission))
                .ToList();
        }

        public EvaluateResult EvaluateRule(EntityNode node, IEnumerable<EntityNodeRuleContainer> rules, Guid permission)
        {
            var filteredRules = rules
                .Where(x =>
                    x.SecurityRule.EntityId == node.Reference.EntityId ||
                    x.SecurityRule.Rule.Inherit)
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
            return new EvaluateResult(node.Reference.EntityId, permission, isGranted);
        }
    }
}
