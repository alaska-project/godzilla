﻿using Godzilla.Abstractions;
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
        private readonly ISecurityDisablerService _securityDisabler;
        private readonly ISecurityRulesFinder<TContext> _rulesFinder;
        private readonly ISecurityRuleMatcher<TContext> _rulesMatcher;
        private readonly ISecurityContext<TContext> _securityContext;
        private readonly ISecurityOptions<TContext> _securityOptions;

        public SecurityRuleEvaluator(
            ISecurityDisablerService securityDisabler,
            ISecurityRulesFinder<TContext> rulesFinder,
            ISecurityRuleMatcher<TContext> rulesMatcher,
            ISecurityContext<TContext> securityContext,
            ISecurityOptions<TContext> securityOptions)
        {
            _securityDisabler = securityDisabler ?? throw new ArgumentNullException(nameof(securityDisabler));
            _rulesFinder = rulesFinder ?? throw new ArgumentNullException(nameof(rulesFinder));
            _rulesMatcher = rulesMatcher ?? throw new ArgumentNullException(nameof(rulesMatcher));
            _securityContext = securityContext ?? throw new ArgumentNullException(nameof(securityContext));
            _securityOptions = securityOptions ?? throw new ArgumentNullException(nameof(securityOptions));
        }

        private bool UseAuthorization
        {
            get
            {
                if (_securityDisabler.IsSecurityDisabled())
                    return false;

                return _securityOptions?.UseAuthorization ?? false;
            }
        }

        public bool IsAuthEnabled() => UseAuthorization;

        public Task<EvaluateResult> EvaluateRoot(Guid permission)
        {
            if (!UseAuthorization ||
                _securityContext.IsAdministrator())
                return Task.FromResult(new EvaluateResult(Guid.Empty, permission, true, null));

            var defaultRule = _securityOptions
                .DefaultSecurityRules
                .First(x => x.Rule.Right == permission);

            var result = new EvaluateResult(
                Guid.Empty,
                permission,
                defaultRule.Rule.Type == RuleType.Allow,
                null);

            return Task.FromResult(result);
        }

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
