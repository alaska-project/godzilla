using Godzilla.Abstractions;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Settings
{
    internal class SecurityOptions<TContext> : ISecurityOptions<TContext>
        where TContext : EntityContext
    {
        public bool UseAuthorization { get; set; } = false;

        public IEnumerable<EntitySecurityRule> DefaultSecurityRules { get; set; } = CreateDefaultAllowRule();

        public IEnumerable<string> AdminRoles { get; set; } = new List<string> { "admin", "administrator" };

        private static IEnumerable<EntitySecurityRule> CreateDefaultAllowRule()
        {
            return CreateDefaultRule(RuleType.Allow, SecurityRight.All);
        }

        private static IEnumerable<EntitySecurityRule> CreateDefaultDenyRule()
        {
            return CreateDefaultRule(RuleType.Deny, SecurityRight.All);
        }

        private static IEnumerable<EntitySecurityRule> CreateDefaultRule(int roleType, IEnumerable<Guid> rights)
        {
            return rights
                .Select(x => new EntitySecurityRule
                {
                    EntityId = Guid.Empty,
                    Subject = new RuleSubject
                    {
                        SubjectType = SubjectType.WellKnownIdentity,
                        SubjectId = WellKnownIdentities.Everyone,
                    },
                    Rule = new SecurityRule
                    {
                        Inherit = true,
                        Type = roleType,
                        Right = x
                    }
                })
                .ToList();
        }
    }
}
