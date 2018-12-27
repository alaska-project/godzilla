using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    internal class MoveEntitiesCommand<TContext> : IRequest<Unit>
        where TContext : EntityContext
    {
        public MoveEntitiesCommand(IEnumerable<object> entities, Guid newParentId)
        {
            Entities = entities;
            NewParentId = newParentId;
        }

        public IEnumerable<object> Entities { get; }
        public Guid NewParentId { get; }
    }
}
