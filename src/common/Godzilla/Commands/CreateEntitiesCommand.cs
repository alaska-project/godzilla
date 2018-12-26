using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    internal class CreateEntitiesCommand<TContext> : IRequest<IEnumerable<object>>
        where TContext : EntityContext
    {
        public CreateEntitiesCommand(Guid parentId, IEnumerable<object> entities)
        {
            ParentId = parentId;
            Entities = entities;
        }

        public Guid ParentId { get; }
        public IEnumerable<object> Entities { get; }
    }
}
