using Godzilla.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    public interface ISecurityImpersonationService
    {
        ImpersonatedSecurityContext ImpersonateAdmin();
        ImpersonatedSecurityContext ImpersonateUser(string userId);
        ImpersonatedPrincipal ImpersonatedPrincipal { get; }
    }
}
