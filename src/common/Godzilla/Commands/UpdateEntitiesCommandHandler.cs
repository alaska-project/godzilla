using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    public class UpdateEntitiesCommandHandler<TContext> : IRequestHandler<UpdateEntitiesCommand<TContext>, IEnumerable<object>>
        where TContext : EntityContext
    {
        public Task<IEnumerable<object>> Handle(UpdateEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
