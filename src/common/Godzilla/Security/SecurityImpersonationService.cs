using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security
{
    internal class SecurityImpersonationService : ISecurityImpersonationService
    {
        private ImpersonatedSecurityContext _impersonatedContext = null;

        public ImpersonatedSecurityContext ImpersonateAdmin()
        {
            _impersonatedContext = new ImpersonatedSecurityContext(new ImpersonatedPrincipal(true, null));
            return _impersonatedContext;
        }

        public ImpersonatedSecurityContext ImpersonateUser(string userId)
        {
            _impersonatedContext = new ImpersonatedSecurityContext(new ImpersonatedPrincipal(false, userId));
            return _impersonatedContext;
        }

        public ImpersonatedPrincipal ImpersonatedPrincipal => _impersonatedContext?.ImpersonatedPrincipal;
    }
}
