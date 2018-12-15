using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    public interface ICollectionInfo
    {
        Type CollectionItemType { get; }
        string CollectionId { get; }
    }
}
