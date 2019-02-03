using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security
{
    internal class SecurityRule
    {
        public string PrincipalId { get; set; }
        public PrincipalType PrincipalType { get; set; }
        public List<SecurityRight> Rights { get; set; }
    }

    public enum PrincipalType
    {
        WellKnownIdentity = 0,
        User = 1,
        Group = 2,
    }

    public enum SecurityRight
    {
        Read = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
    }

    public enum WellKnownIdentity
    {
        Everyone,
        Anonymous
    }
}
