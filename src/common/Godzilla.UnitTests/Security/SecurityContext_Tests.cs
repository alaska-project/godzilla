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

            _securityContext = new SecurityContext<FakeEntityContext>(_securityOptions.Object, _securityContextProvider.Object);
        }

        [Fact]
        public void GetAnonymousApplyableSubjects()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(false);

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

            var isAdmin = _securityContext.IsAdministrator();
            Assert.False(isAdmin);
        }

        [Fact]
        public void AnonymoudNotAdmin()
        {
            _securityContextProvider
                .Setup(x => x.IsAuthenticated)
                .Returns(false);

            var isAdmin = _securityContext.IsAdministrator();
            Assert.False(isAdmin);
        }
    }
}
