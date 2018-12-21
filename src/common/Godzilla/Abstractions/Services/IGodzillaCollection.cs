using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface IGodzillaCollection<TItem>
    {
        IQueryable<TItem> AsQueryable();
        IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : TItem;

        void Add(TItem entity);
        void Add<TDerived>(TDerived entity) where TDerived : TItem;

        void Update(TItem entity);
        void Update<TDerived>(TDerived entity) where TDerived : TItem;

        void Delete(TItem entity);
        void Delete<TDerived>(TItem entity) where TDerived : TItem;
    }

    public interface IGodzillaCollection
    {
        void Add(object entity);
        void Update(object entity);
        void Delete(object entity);
    }
}
