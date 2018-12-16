﻿using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface ITransactionService<TContext>
        where TContext : EntityContext
    {
        void StartTransaction();
        void CommitTransaction();
        void AbortTransaction();

        IGodzillaCollection<TItem> GetCollection<TItem>();
        TCollection GetCollection<TItem, TCollection>() where TCollection : IGodzillaCollection<TItem>;
    }
}
