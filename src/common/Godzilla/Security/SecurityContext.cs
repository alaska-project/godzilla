using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Security
{
    internal class SecurityContext<TContext> : ISecurityContext<TContext>
        where TContext : EntityContext
    {
        private ISecurityOptions<TContext> _securityOptions;
        private ISecurityContextProvider<TContext> _securityContextProvider;

        public SecurityContext(ISecurityOptions<TContext> securityOptions,
            ISecurityContextProvider<TContext> securityContextProvider)
        {
            _securityOptions = securityOptions ?? throw new ArgumentNullException(nameof(securityOptions));
            _securityContextProvider = securityContextProvider;
        }

        public IEnumerable<RuleSubject> GetApplyableSubjects()
        {
            if (!_securityContextProvider.IsAuthenticated)
            {
                return new List<RuleSubject>
                {
                    new RuleSubject
                    {
                        SubjectType = SubjectType.WellKnownIdentity,
                        SubjectId = WellKnownIdentities.Anonymous,
                    },
                    new RuleSubject
                    {
                        SubjectType = SubjectType.WellKnownIdentity,
                        SubjectId = WellKnownIdentities.Everyone,
                    },
                };
            }

            return new List<RuleSubject>
            {
                new RuleSubject
                {
                    SubjectType = SubjectType.WellKnownIdentity,
                    SubjectId = WellKnownIdentities.Authenticated,
                },
                new RuleSubject
                {
                    SubjectType = SubjectType.WellKnownIdentity,
                    SubjectId = WellKnownIdentities.Everyone,
                },
                new RuleSubject
                {
                    SubjectType = SubjectType.User,
                    SubjectId = _securityContextProvider.UserId,
                },
            };
        }

        public bool IsAdministrator()
        {
            return _securityContextProvider.IsAuthenticated &&
                _securityContextProvider.GetRoles().Intersect(_securityOptions.AdminRoles).Any();
        }
    }
}
