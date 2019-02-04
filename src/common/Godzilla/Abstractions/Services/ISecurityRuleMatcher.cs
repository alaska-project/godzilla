using Godzilla.DomainModels;
using Godzilla.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface ISecurityRuleMatcher<TContext>
        where TContext : EntityContext
    {
        EvaluateResult EvaluateRule(EntityNode node, IEnumerable<EntityNodeRuleContainer> rules, Guid permission);
    }
}
