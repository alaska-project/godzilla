using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    internal class RenameEntityCommand<TContext> : IRequest<Unit>
        where TContext : EntityContext
    {
        public RenameEntityCommand(Guid entityId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("message", nameof(newName));
            }

            EntityId = entityId;
            NewName = newName;
        }

        public Guid EntityId { get; }
        public string NewName { get; }
    }
}
