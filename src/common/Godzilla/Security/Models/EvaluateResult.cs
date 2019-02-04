using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Security.Models
{
    internal class EvaluateResult
    {
        public EvaluateResult(Guid entityId, Guid accessRight, bool isRightGranted)
        {
            EntityId = entityId;
            AccessRight = accessRight;
            IsRightGranted = isRightGranted;
        }

        public Guid EntityId { get; }
        public Guid AccessRight { get; }
        public bool IsRightGranted { get; }
    }
}
