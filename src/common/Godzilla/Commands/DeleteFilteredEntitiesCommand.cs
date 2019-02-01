using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Godzilla.Commands
{
    internal class DeleteFilteredEntitiesCommand<TContext, TEntity> : IRequest<Unit>
        where TContext : EntityContext
    {
        public DeleteFilteredEntitiesCommand(Expression<Func<TEntity, bool>> filter)
        {
            Filter = filter;
        }

        public Expression<Func<TEntity, bool>> Filter { get; }
    }
}
