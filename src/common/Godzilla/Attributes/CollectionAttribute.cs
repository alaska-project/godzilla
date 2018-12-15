using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Attributes
{
    public class CollectionAttribute : Attribute
    {
        public CollectionAttribute(string collectionId)
        {
            CollectionId = collectionId;
        }

        public string CollectionId { get; }
    }
}
