using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.DomainModels
{
    internal class TreeEdge
    {
        public Guid Id { get; set; }
        public NodeReference Reference { get; set; }
    }

    internal class NodeReference
    {
        public string CollectionId { get; set; }
        public Guid NodeId { get; set; }
        public Guid ParentId { get; set; }
        public string NodeName { get; set; }
        public string Path { get; set; }
        public string IdPath { get; set; }
    }
}
