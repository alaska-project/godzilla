using Godzilla.Abstractions.Collections;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Collections.Internal
{
    internal class SecurityRulesCollection : GodzillaCollection<EntitySecurityRule>,
        ISecurityRulesCollection
    {
        public SecurityRulesCollection(IDatabaseCollection<EntitySecurityRule> collection)
            : base(collection)
        { }

        public async Task<IEnumerable<EntitySecurityRule>> GetRules(IEnumerable<Guid> entityIds, IEnumerable<RuleSubject> subjects)
        {
            var wellKnownSubjectsId = FilterSubjectId(subjects, SubjectType.WellKnownIdentity);
            var userSubjectsId = FilterSubjectId(subjects, SubjectType.User);
            var groupSubjectsId = FilterSubjectId(subjects, SubjectType.Group);

            return await GetItems(x =>
                entityIds.Contains(x.EntityId) && 
                (
                    x.Subject.SubjectType == SubjectType.WellKnownIdentity &&
                    wellKnownSubjectsId.Contains(x.Subject.SubjectId)
                ) ||
                (
                    x.Subject.SubjectType == SubjectType.User &&
                    userSubjectsId.Contains(x.Subject.SubjectId)
                ) ||
                (
                    x.Subject.SubjectType == SubjectType.Group &&
                    groupSubjectsId.Contains(x.Subject.SubjectId)
                ));
        }

        public async Task SetRule(Guid entityId, RuleSubject subject, SecurityRule rule)
        {
            var entityRule = await GetRule(entityId, subject, rule);
            if (entityRule != null)
            {
                entityRule.Rule = rule;
                await Update(entityRule);
                return;
            }

            await Add(new EntitySecurityRule
            {
                EntityId = entityId,
                Subject = subject,
                Rule = rule,
            });
        }

        public async Task DeleteRule(Guid entityId, RuleSubject subject, SecurityRule rule)
        {
            await Delete(Filter(entityId, subject, rule));
        }

        public async Task DeleteEntityRule(Guid entityId)
        {
            await DeleteEntityRules(new List<Guid> { entityId });
        }

        public async Task DeleteEntityRules(IEnumerable<Guid> entitiesId)
        {
            await Delete(x => entitiesId.Contains(x.EntityId));
        }

        public async Task DeleteSubjectRule(RuleSubject subject)
        {
            await Delete(x =>
                x.Subject.SubjectId == subject.SubjectId &&
                x.Subject.SubjectType == subject.SubjectType);
        }

        public async Task<EntitySecurityRule> GetRule(Guid entityId, RuleSubject subject, SecurityRule rule)
        {
            return await GetItem(Filter(entityId, subject, rule));
        }

        private List<string> FilterSubjectId(IEnumerable<RuleSubject> subjects, int subjectType)
        {
            return subjects
                .Where(x => x.SubjectType == subjectType)
                .Select(x => x.SubjectId)
                .ToList();
        }

        private Expression<Func<EntitySecurityRule, bool>> Filter(Guid entityId, RuleSubject subject, SecurityRule rule)
        {
            return x =>
                x.EntityId == entityId &&
                x.Subject.SubjectId == subject.SubjectId &&
                x.Subject.SubjectType == subject.SubjectType &&
                x.Rule.Right == rule.Right;
        }
    }
}
