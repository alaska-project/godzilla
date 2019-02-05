using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security
{
    public class ImpersonatedSecurityContext : IDisposable
    {
        private ImpersonatedPrincipal _impersonatedPrincipal;

        public ImpersonatedSecurityContext(ImpersonatedPrincipal impersonatedPrincipal)
        {
            _impersonatedPrincipal = impersonatedPrincipal;
        }

        public ImpersonatedPrincipal ImpersonatedPrincipal => _impersonatedPrincipal;

        public void Dispose()
        {
            _impersonatedPrincipal = null;
        }
    }

    public class ImpersonatedPrincipal
    {
        public ImpersonatedPrincipal(bool isAdmin, string userId)
        {
            IsAdmin = isAdmin;
            UserId = userId;
        }

        public bool IsAdmin { get; }
        public string UserId { get; }
        public bool IsAuthenticated => IsAdmin || !string.IsNullOrEmpty(UserId);
    }
}
