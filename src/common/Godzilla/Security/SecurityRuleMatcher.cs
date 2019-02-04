using Godzilla.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security
{
    internal class SecurityRuleMatcher<TContext> : ISecurityRuleMatcher<TContext>
        where TContext : EntityContext
    {
    }
}
