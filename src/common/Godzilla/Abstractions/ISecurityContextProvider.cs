using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface ISecurityContextProvider<TContext>
        where TContext : EntityContext
    {
        bool IsAuthenticated { get; }
        string UserId { get; }
        IEnumerable<string> GetRoles();
    }
}
