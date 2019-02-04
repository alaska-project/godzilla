using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security.Models
{
    internal class EvaluateResult
    {
        public EvaluateResult(Guid entityId, Guid accessRight, bool isRightGranted, EntitySecurityRule matchedRule)
        {
            EntityId = entityId;
            AccessRight = accessRight;
            IsRightGranted = isRightGranted;
            MatchedRule = matchedRule;
        }

        public Guid EntityId { get; }
        public Guid AccessRight { get; }
        public bool IsRightGranted { get; }
        public EntitySecurityRule MatchedRule { get; }
    }
}
