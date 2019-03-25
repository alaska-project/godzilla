using Godzilla.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    /// <summary>
    /// This service can be used to explicitly run a code section as if it is invoked by a specific identity
    /// </summary>
    public interface ISecurityImpersonationService
    {
        ImpersonatedSecurityContext ImpersonateAdmin();
        ImpersonatedSecurityContext ImpersonateUser(string userId);
        ImpersonatedPrincipal ImpersonatedPrincipal { get; }
    }
}
