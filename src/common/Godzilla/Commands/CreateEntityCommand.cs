using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    public class CreateEntityCommand<TContext> : IRequest<object>
        where TContext : EntityContext
    {
        public CreateEntityCommand(Guid parentId, object entity)
        {
            ParentId = parentId;
            Entity = entity;
        }

        public Guid ParentId { get; }
        public object Entity { get; }
    }
}
