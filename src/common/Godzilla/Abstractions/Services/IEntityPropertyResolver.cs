using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IEntityPropertyResolver<TContext> : IEntityPropertyResolver
        where TContext : EntityContext
    { }

    public interface IEntityPropertyResolver
    {
        Guid GetEntityId(object entity);
        Guid GetEntityId(object entity, bool generateIfEmpty);
        string GetEntityName(object entity);
    }
}
