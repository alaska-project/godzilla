using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    internal class DeleteEntitiesCommand<TContext> : IRequest<Unit>
        where TContext : EntityContext
    {
        public DeleteEntitiesCommand(IEnumerable<object> entities)
        {
            Entities = entities;
        }

        public IEnumerable<object> Entities { get; }
    }
}
