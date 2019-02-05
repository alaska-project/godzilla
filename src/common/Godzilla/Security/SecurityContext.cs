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
        private ISecurityImpersonationService _impersonationService;

        public SecurityContext(ISecurityOptions<TContext> securityOptions,
            ISecurityImpersonationService impersonationService,
            ISecurityContextProvider<TContext> securityContextProvider = null)
        {
            _securityOptions = securityOptions ?? throw new ArgumentNullException(nameof(securityOptions));
            _impersonationService = impersonationService ?? throw new ArgumentNullException(nameof(impersonationService));
            _securityContextProvider = securityContextProvider;
        }

        public IEnumerable<RuleSubject> GetApplyableSubjects()
        {
            if (!IsAuthenticated())
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
                    SubjectId = GetUserId(),
                },
            };
        }

        public bool IsAuthenticated()
        {
            if (_impersonationService.ImpersonatedPrincipal != null)
                return _impersonationService.ImpersonatedPrincipal.IsAuthenticated;

            return _securityContextProvider.IsAuthenticated;
        }

        public string GetUserId()
        {
            if (_impersonationService.ImpersonatedPrincipal != null)
                return _impersonationService.ImpersonatedPrincipal.UserId;

            return _securityContextProvider.UserId;
        }
        
        public bool IsAdministrator()
        {
            if (_impersonationService.ImpersonatedPrincipal != null)
                return _impersonationService.ImpersonatedPrincipal.IsAdmin;

            return _securityContextProvider.IsAuthenticated &&
                _securityContextProvider.GetRoles().Intersect(_securityOptions.AdminRoles).Any();
        }
    }
}
