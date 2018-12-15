using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.DomainModels
{
    internal class TreeEdge
    {
        public Guid ParentId { get; set; }
        public Guid NodeId { get; set; }
        public string NodeName { get; set; }
    }
}
