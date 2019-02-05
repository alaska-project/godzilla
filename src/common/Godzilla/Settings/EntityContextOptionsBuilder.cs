using Godzilla.Abstractions;
using Godzilla.DomainModels;
using Godzilla.Settings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public class EntityContextOptionsBuilder<TContext>
        where TContext : EntityContext
    {
        private readonly SecurityOptions<TContext> _securityOptions;

        internal EntityContextOptionsBuilder(IGodzillaServiceBuilder builder, SecurityOptions<TContext> securityOptions)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _securityOptions = securityOptions ?? throw new ArgumentNullException(nameof(securityOptions));
        }

        public IGodzillaServiceBuilder Builder { get; }

        public void UseAuthorization()
        {
            _securityOptions.UseAuthorization = true;
        }

        public void SetDenyAllDefaultSecurityRule()
        {
            _securityOptions.DefaultSecurityRules = SecurityOptions<TContext>.CreateDefaultDenyRule();
        }

        public void SetDenyAllowDefaultSecurityRule()
        {
            _securityOptions.DefaultSecurityRules = SecurityOptions<TContext>.CreateDefaultAllowRule();
        }
    }
}
