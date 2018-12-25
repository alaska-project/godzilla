using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    public class UpdateEntitiesCommand<TContext> : IRequest<IEnumerable<object>>
        where TContext : EntityContext
    {
        public UpdateEntitiesCommand(IEnumerable<object> entities)
        {
            Entities = entities;
        }

        public IEnumerable<object> Entities { get; }
    }
}
