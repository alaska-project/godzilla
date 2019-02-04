using Godzilla.DomainModels;
using Godzilla.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    internal interface ISecurityRulesFinder<TContext>
        where TContext : EntityContext
    {
        Task<IEnumerable<EntityNodeRules>> GetRules(IEnumerable<Guid> entitiesId, IEnumerable<RuleSubject> subjects, Guid accessRight);
    }
}
