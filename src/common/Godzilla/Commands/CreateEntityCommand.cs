using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    public class CreateEntityCommand<TContext> : IRequest<bool>
        where TContext : EntityContext
    {
    }
}
