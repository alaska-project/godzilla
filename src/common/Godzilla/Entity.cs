using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public abstract class Entity
    {
        private EntityContext _context;

        internal void SetContext(EntityContext context)
        {
            _context = context;
        }
    }
}
