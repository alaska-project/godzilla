using Godzilla.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security
{
    internal class SecurityDisablerService : ISecurityDisablerService
    {
        private SecurityDisabler _securityDisabler = new SecurityDisabler(false);

        public IDisposable DisableSecurity()
        {
            var disabler = new SecurityDisabler();
            _securityDisabler = disabler;
            return disabler;
        }

        public bool IsSecurityDisabled()
        {
            return _securityDisabler.IsSecurityDisabled();
        }
    }
}
