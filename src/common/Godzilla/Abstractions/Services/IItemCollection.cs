using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface IItemCollection
    {
        void AddItem(object item);
        void UpdateItem(object item);
        void DeleteItem(object item);
    }
}
