using System;
using System.Collections.Generic;
using System.Linq;
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

    public class SecurityRule : IEquatable<SecurityRule>
    {
        internal SecurityRule()
        { }

        internal SecurityRule(Guid right, int type, bool inherit)
        {
            Right = right;
            Type = type;
            Inherit = inherit;
        }

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

        #region Allow Rules

        public static SecurityRule AllowRead(bool inherit)
        {
            return new SecurityRule(SecurityRight.Read, RuleType.Allow, inherit);
        }

        public static SecurityRule AllowMove(bool inherit)
        {
            return new SecurityRule(SecurityRight.Move, RuleType.Allow, inherit);
        }

        public static SecurityRule AllowRename(bool inherit)
        {
            return new SecurityRule(SecurityRight.Rename, RuleType.Allow, inherit);
        }

        public static SecurityRule AllowUpdate(bool inherit)
        {
            return new SecurityRule(SecurityRight.Update, RuleType.Allow, inherit);
        }

        public static SecurityRule AllowDelete(bool inherit)
        {
            return new SecurityRule(SecurityRight.Delete, RuleType.Allow, inherit);
        }

        public static IEnumerable<SecurityRule> AllowAll(bool inherit)
        {
            return SecurityRight.All
                .Select(x => new SecurityRule(x, RuleType.Allow, inherit))
                .ToList();
        }

        #endregion

        #region Deny Rules

        public static SecurityRule DenyRead(bool inherit)
        {
            return new SecurityRule(SecurityRight.Read, RuleType.Deny, inherit);
        }

        public static SecurityRule DenyMove(bool inherit)
        {
            return new SecurityRule(SecurityRight.Move, RuleType.Deny, inherit);
        }

        public static SecurityRule DenyRename(bool inherit)
        {
            return new SecurityRule(SecurityRight.Rename, RuleType.Deny, inherit);
        }

        public static SecurityRule DenyUpdate(bool inherit)
        {
            return new SecurityRule(SecurityRight.Update, RuleType.Deny, inherit);
        }

        public static SecurityRule DenyDelete(bool inherit)
        {
            return new SecurityRule(SecurityRight.Delete, RuleType.Deny, inherit);
        }

        public static IEnumerable<SecurityRule> DenyAll(bool inherit)
        {
            return SecurityRight.All
                .Select(x => new SecurityRule(x, RuleType.Deny, inherit))
                .ToList();
        }

        #endregion
    }

    public class RuleSubject : IEquatable<RuleSubject>
    {
        internal RuleSubject()
        { }

        public string SubjectId { get; set; }
        public int SubjectType { get; set; }

        public bool Equals(RuleSubject other)
        {
            return
                SubjectId.Equals(other.SubjectId) &&
                SubjectType == other.SubjectType;
        }

        public static RuleSubject User(string userId)
        {
            return new RuleSubject
            {
                SubjectId = userId,
                SubjectType = DomainModels.SubjectType.User,
            };
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
        public static readonly Guid Rename = new Guid("b94bbcc3-e844-4231-8d80-e73092751837");
        public static readonly Guid Move = new Guid("32f32999-8223-4b3d-afe3-40af5c471493");
        public static readonly Guid Delete = new Guid("1b7f310e-27ad-4886-ba7c-929a61e3515e");
        public static readonly Guid Administer = new Guid("ef383f3f-70ba-426c-86df-2d622916bdb3");

        public static IEnumerable<Guid> All => new List<Guid>
        {
            Read,
            Create,
            Update,
            Rename,
            Move,
            Delete,
            Administer
        };
    }

    internal static class WellKnownIdentities
    {
        public const string Everyone = "ba6ebb6b-3073-4121-9a06-0ad36d82a8a7";
        public const string Anonymous = "bbb1487b-20e1-4e29-8650-0606b0d4d714";
        public const string Authenticated = "41d23e57-a749-4df0-bdd7-fe15bfffaa3b";
    }
}
