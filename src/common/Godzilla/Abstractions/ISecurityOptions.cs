using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    internal interface ISecurityOptions<TContext>
        where TContext : EntityContext
    {
        bool UseAuthorization { get; }
        IEnumerable<EntitySecurityRule> DefaultSecurityRules { get; }
        IEnumerable<string> AdminRoles { get; }
    }
}
