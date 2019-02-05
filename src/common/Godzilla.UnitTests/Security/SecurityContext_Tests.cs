using Godzilla.Abstractions;
using Godzilla.DomainModels;
using Godzilla.Security;
using Godzilla.Settings;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Godzilla.UnitTests.Security
{
    public class SecurityContext_Tests
    {
        private SecurityContext<FakeEntityContext> _securityContext;
        private Mock<ISecurityImpersonationService> _impersonationService = new Mock<ISecurityImpersonationService>();
        private Mock<ISecurityOptions<FakeEntityContext>> _securityOptions = new Mock<ISecurityOptions<FakeEntityContext>>();
        private Mock<ISecurityContextProvider<FakeEntityContext>> _securityContextProvider = new Mock<ISecurityContextProvider<FakeEntityContext>>();

        public SecurityContext_Tests()
        {
            _securityOptions
                .Setup(x => x.AdminRoles)
                .Returns(() => new List<string> { "admin" });

            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(() => true);

            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(() => SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns((ImpersonatedPrincipal)null);

            _securityContext = new SecurityContext<FakeEntityContext>(_securityOptions.Object, _impersonationService.Object, _securityContextProvider.Object);
        }

        [Fact]
        public void GetAnonymousApplyableSubjects()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(false);

            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns((ImpersonatedPrincipal)null);

            var result = _securityContext
                .GetApplyableSubjects();

            Assert.Equal(new List<RuleSubject>
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
            }, result);
        }

        [Fact]
        public void GetLoggedApplyableSubjects()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(true);

            _securityContextProvider
                .Setup(x => x.UserId)
                .Returns("User1");

            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns((ImpersonatedPrincipal)null);

            var result = _securityContext
                .GetApplyableSubjects();

            Assert.Equal(new List<RuleSubject>
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
                    SubjectId = "User1",
                },
            }, result);
        }

        [Fact]
        public void IsAdmin()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(true);

            _securityContextProvider
                .Setup(x => x.GetRoles())
                .Returns(new List<string> { "admin" });

            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns((ImpersonatedPrincipal)null);

            var isAdmin = _securityContext.IsAdministrator();
            Assert.True(isAdmin);
        }

        [Fact]
        public void LoggedNotAdmin()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(true);

            _securityContextProvider
                .Setup(x => x.GetRoles())
                .Returns(new List<string> { "user" });

            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns((ImpersonatedPrincipal)null);

            var isAdmin = _securityContext.IsAdministrator();
            Assert.False(isAdmin);
        }

        [Fact]
        public void AnonymousNotAdmin()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(false);

            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns((ImpersonatedPrincipal)null);

            var isAdmin = _securityContext.IsAdministrator();
            Assert.False(isAdmin);
        }

        [Fact]
        public void ImpersonatedAdmin()
        {
            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns(new ImpersonatedPrincipal(true, ""));

            var isAdmin = _securityContext.IsAdministrator();
            Assert.True(isAdmin);
        }

        [Fact]
        public void ImpersonatedAnonymousNonAdmin()
        {
            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns(new ImpersonatedPrincipal(false, ""));

            var isAdmin = _securityContext.IsAdministrator();
            var isAuthenticated = _securityContext.IsAuthenticated();

            Assert.False(isAdmin);
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void ImpersonatedAnonymous()
        {
            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns(new ImpersonatedPrincipal(false, ""));

            var isAdmin = _securityContext.IsAdministrator();
            var isAuthenticated = _securityContext.IsAuthenticated();

            Assert.False(isAdmin);
            Assert.False(isAuthenticated);
        }

        [Fact]
        public void ImpersonatedUser()
        {
            _impersonationService
                .Setup(x => x.ImpersonatedPrincipal)
                .Returns(new ImpersonatedPrincipal(false, "User1"));

            var isAdmin = _securityContext.IsAdministrator();
            var isAuthenticated = _securityContext.IsAuthenticated();
            var userId = _securityContext.GetUserId();

            Assert.False(isAdmin);
            Assert.True(isAuthenticated);
            Assert.Equal("User1", userId);
        }
    }
}
