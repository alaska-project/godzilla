using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security
{
    public class SecurityDisabler : IDisposable
    {
        private bool _securityDisabled;

        public SecurityDisabler()
            : this(true)
        { }

        public SecurityDisabler(bool securityDisabled)
        {
            _securityDisabled = securityDisabled;
        }

        public bool IsSecurityDisabled() => _securityDisabled;

        public void Dispose()
        {
            _securityDisabled = false;
        }
    }
}
