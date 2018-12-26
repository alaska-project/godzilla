using Godzilla.Abstractions.Collections;
using Godzilla.Collections.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface IEntityCommandsHelper<TContext>
        where TContext : EntityContext
    {
        Type GetEntityType(IEnumerable<object> entities);
        void VerifyEntitiesExist(ITreeEdgesCollection edgesCollection);
    }
}
