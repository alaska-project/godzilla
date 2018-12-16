using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface ICollectionResolver<TContext>
        where TContext : EntityContext
    {
        ICollectionInfo GetCollectionInfo<TItem>();
    }
}
