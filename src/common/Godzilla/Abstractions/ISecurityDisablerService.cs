using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions
{
    /// <summary>
    /// This service can be used for explicitly disable security evaluation inside its scope
    /// </summary>
    public interface ISecurityDisablerService
    {
        bool IsSecurityDisabled();
        IDisposable DisableSecurity();
    }
}
