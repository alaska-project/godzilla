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
                    .Select(x => new EvaluateResult(x, permission, true, null));

            var appliableSubjects = _securityContext.GetApplyableSubjects();

            var rules = await _rulesFinder.GetRules(entitiesId, appliableSubjects, permission);

            return rules
                .Select(x => _rulesMatcher.EvaluateRule(x.Node, x.Rules, permission))
                .ToList();
        }
    }
}
