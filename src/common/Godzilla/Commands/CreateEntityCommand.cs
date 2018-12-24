using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    public class CreateEntityCommand<TContext> : IRequest<IEnumerable<object>>
        where TContext : EntityContext
    {
        public CreateEntityCommand(Guid parentId, IEnumerable<object> entities)
        {
            ParentId = parentId;
            Entities = entities;
        }

        public Guid ParentId { get; }
        public IEnumerable<object> Entities { get; }
    }
}
