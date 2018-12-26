using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class DeleteEntitiesCommandHandler<TContext> : IRequestHandler<DeleteEntitiesCommand<TContext>>
        where TContext : EntityContext
    {
        public Task<Unit> Handle(DeleteEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
