using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    internal class MoveEntityCommand<TContext> : IRequest<Unit>
        where TContext : EntityContext
    {
        public MoveEntityCommand(Guid entityId, Guid newParentId)
        {
            EntityId = entityId;
            NewParentId = newParentId;
        }

        public Guid EntityId { get; }
        public Guid NewParentId { get; }
    }
}
