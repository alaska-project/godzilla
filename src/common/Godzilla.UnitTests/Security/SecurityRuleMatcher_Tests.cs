using Godzilla.Abstractions;
using Godzilla.DomainModels;
using Godzilla.Security;
using Godzilla.Security.Models;
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
        public void Evaluate_DefaultDenyRule_ExpectDenyRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var entityId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{Guid.NewGuid()}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>();

            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.False(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.Equal(Guid.Empty, result.MatchedRule.Id);
        }
        
        [Fact]
        public void EvaluateSpecificRule_DefaultDeny_ExpectAllowRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{Guid.NewGuid()}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = entityId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Allow,
                        Inherit = false,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.True(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(ruleId, result.MatchedRule.Id);
        }

        [Fact]
        public void EvaluateSpecificRule_DefaultDeny_ExpectDenyUpdate()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{Guid.NewGuid()}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = entityId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Allow,
                        Inherit = false,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Update);

            Assert.False(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Update, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(Guid.Empty, result.MatchedRule.Id);
        }

        [Fact]
        public void EvaluateInheritedRule_DefaultDeny_ExpectAllowRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{parentId}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = parentId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Allow,
                        Inherit = true,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.True(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(ruleId, result.MatchedRule.Id);
            Assert.Equal(parentId, result.MatchedRule.EntityId);
        }

        [Fact]
        public void EvaluateNonInheritedRule_DefaultDeny_ExpectDenyRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultDenyRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{parentId}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = parentId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Allow,
                        Inherit = false,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.False(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(Guid.Empty, result.MatchedRule.Id);
        }

        [Fact]
        public void Evaluate_DefaultAllowRule_ExpectAllowRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            var entityId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{Guid.NewGuid()}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>();

            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.True(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.Equal(Guid.Empty, result.MatchedRule.Id);
        }

        [Fact]
        public void EvaluateSpecificRule_DefaultAllow_ExpectDenyRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{Guid.NewGuid()}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = Guid.NewGuid(),
                    EntityId = entityId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Allow,
                        Inherit = false,
                    }
                }),
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = entityId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Deny,
                        Inherit = false,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.False(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(ruleId, result.MatchedRule.Id);
        }

        [Fact]
        public void EvaluateSpecificRule_DefaultAllow_ExpectAllowUpdate()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{Guid.NewGuid()}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = entityId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Deny,
                        Inherit = false,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Update);

            Assert.True(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Update, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(Guid.Empty, result.MatchedRule.Id);
        }

        [Fact]
        public void EvaluateInheritedRule_DefaultAllow_ExpectDenyRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{parentId}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = parentId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Deny,
                        Inherit = true,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.False(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(ruleId, result.MatchedRule.Id);
            Assert.Equal(parentId, result.MatchedRule.EntityId);
        }

        [Fact]
        public void EvaluateNonInheritedRule_DefaultAllow_ExpectAllowRead()
        {
            _securityOptions
                .Setup(x => x.DefaultSecurityRules)
                .Returns(SecurityOptions<FakeEntityContext>.CreateDefaultAllowRule());

            var ruleId = Guid.NewGuid();
            var entityId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var node = new EntityNode
            {
                EntityId = entityId,
                IdPath = $"/{Guid.NewGuid()}/{parentId}/{entityId}",
            };
            var rules = new List<EntityNodeRuleContainer>
            {
                new EntityNodeRuleContainer(node, new EntitySecurityRule
                {
                    Id = ruleId,
                    EntityId = parentId,
                    Rule = new SecurityRule
                    {
                        Right = SecurityRight.Read,
                        Type = RuleType.Deny,
                        Inherit = false,
                    }
                })
            };
            var result = _matcher.EvaluateRule(node, rules, SecurityRight.Read);

            Assert.True(result.IsRightGranted);
            Assert.Equal(entityId, result.EntityId);
            Assert.Equal(SecurityRight.Read, result.AccessRight);
            Assert.NotNull(result.MatchedRule);
            Assert.Equal(Guid.Empty, result.MatchedRule.Id);
        }

    }
}
