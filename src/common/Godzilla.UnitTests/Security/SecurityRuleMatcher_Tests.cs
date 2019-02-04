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
    public class SecurityRuleMatcher_Tests
    {
        private SecurityRuleMatcher<FakeEntityContext> _matcher;
        private Mock<ISecurityOptions<FakeEntityContext>> _securityOptions = new Mock<ISecurityOptions<FakeEntityContext>>();

        public SecurityRuleMatcher_Tests()
        {
            _matcher = new SecurityRuleMatcher<FakeEntityContext>(_securityOptions.Object);
        }

        [Fact]
        public void EvaluateSpecificAllowRule()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var node = new EntityNode
            {
                
            };
            //_matcher.EvaluateRule(node, )
        }
    }
}
