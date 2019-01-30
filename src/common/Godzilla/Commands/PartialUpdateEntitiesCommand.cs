using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Godzilla.Commands
{
    internal class PartialUpdateEntitiesCommand<TContext, TEntity, TField> : IRequest<IEnumerable<TEntity>>
        where TContext : EntityContext
    {
        public PartialUpdateEntitiesCommand(IEnumerable<Guid> entities, Expression<Func<TEntity, TField>> field, TField value)
        {
            Entities = entities;
            Field = field;
            Value = value;
        }

        public IEnumerable<Guid> Entities { get; }
        public Expression<Func<TEntity, TField>> Field { get; }
        public TField Value { get; }
    }
}
