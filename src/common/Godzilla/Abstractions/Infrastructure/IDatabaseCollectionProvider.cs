﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IDatabaseCollectionProvider<TContext>
        where TContext : EntityContext
    {
        IDatabaseCollection<TItem> GetCollection<TItem>(string collectionId);
    }
}
