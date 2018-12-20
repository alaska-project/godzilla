using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Infrastructure
{
    public interface IDatabaseCollection<TItem>
    {
        IQueryable<TItem> AsQueryable();
        IQueryable<TDerived> AsQueryable<TDerived>() where TDerived : TItem;

        Task Add(TItem entity);
        Task Add<TDerived>(TDerived entity) where TDerived: TItem;

        void Update(TItem entity);
        void Update<TDerived>(TDerived entity) where TDerived : TItem;

        void Delete(TItem entity);
        void Delete<TDerived>(TItem entity) where TDerived : TItem;
    }
}
