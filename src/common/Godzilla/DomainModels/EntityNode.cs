using Godzilla.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.DomainModels
{
    internal class EntityNode
    {
        public Guid Id { get; set; }
        public NodeReference Reference { get; set; }
        public SecurityRules Security { get; set; }
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

    internal class SecurityRules
    {
        public List<SecurityRule> Rules { get; set; }
    }
}
