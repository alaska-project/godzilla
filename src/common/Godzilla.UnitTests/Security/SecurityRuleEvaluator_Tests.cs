using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.DomainModels;
using Godzilla.Security;
using Godzilla.Security.Models;
using Godzilla.Settings;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Godzilla.UnitTests.Security
{
    public class SecurityRuleEvaluator_Tests
    {
        private SecurityRuleEvaluator<FakeEntityContext> _evaluator;
        private Mock<ISecurityDisablerService> _securityDisablerService = new Mock<ISecurityDisablerService>();
        private Mock<ISecurityRulesFinder<FakeEntityContext>> _rulesFinder = new Mock<ISecurityRulesFinder<FakeEntityContext>>();
        private Mock<ISecurityRuleMatcher<FakeEntityContext>> _rulesMatcher = new Mock<ISecurityRuleMatcher<FakeEntityContext>>();
        private Mock<ISecurityContext<FakeEntityContext>> _securityContext = new Mock<ISecurityContext<FakeEntityContext>>();
        private Mock<ISecurityOptions<FakeEntityContext>> _securityOptions = new Mock<ISecurityOptions<FakeEntityContext>>();

        public SecurityRuleEvaluator_Tests()
        {
            _evaluator = new SecurityRuleEvaluator<FakeEntityContext>(
                _securityDisablerService.Object,
                _rulesFinder.Object,
                _rulesMatcher.Object,
                _securityContext.Object,
                _securityOptions.Object);
        }

        [Fact]
        public async Task EvaluateRootDefaultAllow()
        {
            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(true);

            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            var result = await _evaluator.EvaluateRoot(SecurityRight.Read);

            Assert.True(result.IsRightGranted);
        }

        [Fact]
        public async Task EvaluateRootDefaultDeny()
        {
            _securityDisablerService
                .Setup(x => x.IsSecurityDisabled())
                .Returns(false);

            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(true);

            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var result = await _evaluator.EvaluateRoot(SecurityRight.Read);

            Assert.False(result.IsRightGranted);
        }

        [Fact]
        public async Task EvaluateWithDisabledAuthentication()
        {
            _securityDisablerService
                .Setup(x => x.IsSecurityDisabled())
                .Returns(false);

            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(false);

            var entityId = Guid.NewGuid();
            var results = await _evaluator.Evaluate(new List<Guid> { entityId }, SecurityRight.Read);

            Assert.True(results.Count() == 1);
            Assert.True(results.First().IsRightGranted);
            Assert.Equal(entityId, results.First().EntityId);
        }

        [Fact]
        public async Task EvaluateForAdmin()
        {
            _securityDisablerService
                .Setup(x => x.IsSecurityDisabled())
                .Returns(false);

            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(true);

            _securityContext
                .Setup(x => x.IsAdministrator())
                .Returns(true);

            var entityId = Guid.NewGuid();
            var results = await _evaluator.Evaluate(new List<Guid> { entityId }, SecurityRight.Read);

            Assert.True(results.Count() == 1);
            Assert.True(results.First().IsRightGranted);
            Assert.Equal(entityId, results.First().EntityId);
        }

        [Fact]
        public async Task EvaluateForUser_ReturnsAllow()
        {
            _securityDisablerService
                .Setup(x => x.IsSecurityDisabled())
                .Returns(false);

            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(true);

            _securityContext
                .Setup(x => x.IsAdministrator())
                .Returns(false);

            var entityId = Guid.NewGuid();

            _rulesFinder
                .Setup(x => x.GetRules(
                    It.Is<IEnumerable<Guid>>(p => p.Count() == 1 && p.First() == entityId),
                    It.IsAny<IEnumerable<RuleSubject>>(),
                    It.Is<Guid>(p => p == SecurityRight.Read)))
                .ReturnsAsync(new List<EntityNodeRules> { new EntityNodeRules(null, null) });

            _rulesMatcher
                .Setup(x => x.EvaluateRule(
                    It.IsAny<EntityNode>(),
                    It.IsAny<IEnumerable<EntityNodeRuleContainer>>(),
                    It.IsAny<Guid>()))
                .Returns(new EvaluateResult(entityId, SecurityRight.Read, true, null));

            var results = await _evaluator.Evaluate(new List<Guid> { entityId }, SecurityRight.Read);

            Assert.True(results.Count() == 1);
            Assert.True(results.First().IsRightGranted);
            Assert.Equal(entityId, results.First().EntityId);
        }

        [Fact]
        public async Task EvaluateForUser_ReturnsDeny()
        {
            _securityDisablerService
                .Setup(x => x.IsSecurityDisabled())
                .Returns(false);

            _securityOptions
                .Setup(x => x.UseAuthorization)
                .Returns(true);

            _securityContext
                .Setup(x => x.IsAdministrator())
                .Returns(false);

            var entityId = Guid.NewGuid();

            _rulesFinder
                .Setup(x => x.GetRules(
                    It.Is<IEnumerable<Guid>>(p => p.Count() == 1 && p.First() == entityId),
                    It.IsAny<IEnumerable<RuleSubject>>(),
                    It.Is<Guid>(p => p == SecurityRight.Read)))
                .ReturnsAsync(new List<EntityNodeRules> { new EntityNodeRules(null, null) });

            _rulesMatcher
                .Setup(x => x.EvaluateRule(
                    It.IsAny<EntityNode>(),
                    It.IsAny<IEnumerable<EntityNodeRuleContainer>>(),
                    It.IsAny<Guid>()))
                .Returns(new EvaluateResult(entityId, SecurityRight.Read, false, null));

            var results = await _evaluator.Evaluate(new List<Guid> { entityId }, SecurityRight.Read);

            Assert.True(results.Count() == 1);
            Assert.False(results.First().IsRightGranted);
            Assert.Equal(entityId, results.First().EntityId);
        }
    }
}
