using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.DomainModels
{
    internal class EntitySecurityRule
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public RuleSubject Subject { get; set; }
        public SecurityRule Rule { get; set; }
    }

    internal class SecurityRule : IEquatable<SecurityRule>
    {
        public Guid Right { get; set; }
        public int Type { get; set; }
        public bool Inherit { get; set; }

        public bool Equals(SecurityRule other)
        {
            return
                Right.Equals(other.Right) &&
                Type.Equals(other.Type) &&
                Inherit.Equals(other.Inherit);
        }
    }

    public class RuleSubject : IEquatable<RuleSubject>
    {
        public string SubjectId { get; set; }
        public int SubjectType { get; set; }

        public bool Equals(RuleSubject other)
        {
            return
                SubjectId.Equals(other.SubjectId) &&
                SubjectType == other.SubjectType;
        }
    }

    internal static class RuleType
    {
        public static readonly int Allow = 0;
        public static readonly int Deny = 1;
    }

    internal static class SubjectType
    {
        public static readonly int WellKnownIdentity = 0;
        public static readonly int User = 1;
        public static readonly int Group = 2;
    }

    internal static class SecurityRight
    {
        public static readonly Guid Read = new Guid("ec98809d-5e89-4148-98c4-00ad8d11d050");
        public static readonly Guid Create = new Guid("6871cdb1-b647-4ad9-9b09-5242826c2bcb");
        public static readonly Guid Update = new Guid("9e4d7101-601c-46da-8994-d3666e8c7f6d");
        public static readonly Guid Delete = new Guid("1b7f310e-27ad-4886-ba7c-929a61e3515e");

        public static IEnumerable<Guid> All => new List<Guid>
        {
            Read,
            Create,
            Update,
            Delete
        };
    }

    internal static class WellKnownIdentities
    {
        public const string Everyone = "ba6ebb6b-3073-4121-9a06-0ad36d82a8a7";
        public const string Anonymous = "bbb1487b-20e1-4e29-8650-0606b0d4d714";
        public const string Authenticated = "41d23e57-a749-4df0-bdd7-fe15bfffaa3b";
    }
}
