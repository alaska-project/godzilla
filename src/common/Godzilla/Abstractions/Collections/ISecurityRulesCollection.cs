using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Collections
{
    internal interface ISecurityRulesCollection
    {
        Task<EntitySecurityRule> GetRule(Guid entityId, RuleSubject subject, SecurityRule rule);
        Task<IEnumerable<EntitySecurityRule>> GetRules(IEnumerable<Guid> entityIds, IEnumerable<RuleSubject> subjects);
        Task SetRule(Guid entityId, RuleSubject subject, SecurityRule rule);
        Task DeleteRule(Guid entityId, RuleSubject subject, SecurityRule rule);
        Task DeleteEntityRule(Guid entityId);
        Task DeleteEntityRules(IEnumerable<Guid> entitiesId);
        Task DeleteSubjectRule(RuleSubject subject);

    }
}
