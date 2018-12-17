using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Godzilla.Abstractions.Infrastructure;
using Godzilla.Abstractions.Services;
using Godzilla.Utils;

namespace Godzilla.Collections.Internal
{
    internal class ItemsCollection<TItem> : GodzillaCollection<TItem>,
        IItemCollection
    {
        private readonly MethodInfo _add;
        private readonly MethodInfo _update;
        private readonly MethodInfo _delete;

        public ItemsCollection(IDatabaseCollection<TItem> collection)
            : base(collection)
        {
            _add = GetGenericOperatonMethod("Add");
            _update = GetGenericOperatonMethod("Update");
            _delete = GetGenericOperatonMethod("Delete");
        }

        public void AddItem(object item)
        {
            _add.Invoke(this, new object[] { item });
        }

        public void UpdateItem(object item)
        {
            _update.Invoke(this, new object[] { item });
        }

        public void DeleteItem(object item)
        {
            _delete.Invoke(this, new object[] { item });
        }

        private MethodInfo GetGenericOperatonMethod(string methodName)
        {
            var method = ReflectionUtil.GetGenericMethod(this.GetType(), methodName, BindingFlags.Public | BindingFlags.Instance);
            if (method == null)
                throw new MissingMethodException($"Generic method {methodName} not found");

            return method;
        }
    }
}
