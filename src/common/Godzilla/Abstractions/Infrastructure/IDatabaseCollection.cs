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

        Task Add(TItem entity);
        Task Add(IEnumerable<TItem> entities);

        Task Update(TItem entity);
        Task Update(IEnumerable<TItem> entities);

        Task Delete(TItem entity);
        Task Delete(IEnumerable<TItem> entities);
    }
}
