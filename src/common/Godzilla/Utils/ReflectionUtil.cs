using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Godzilla.Utils
{
    internal static class ReflectionUtil
    {
        public static IEnumerable<Type> GetBaseTypesAndSelf(Type type)
        {
            return new List<Type> { type }
                .Union(GetBaseTypes(type));
        }

        public static IEnumerable<Type> GetBaseTypes(Type type)
        {
            while (type.BaseType != null && 
                type.BaseType != typeof(object))
            {
                yield return type.BaseType;
                type = type.BaseType;
            }
        }

        public static MethodInfo GetGenericMethod(Type type, string name, BindingFlags bindingFlags)
        {
            return GetGenericMethod(type, name, bindingFlags, 1, null);
        }

        public static MethodInfo GetGenericMethod(Type type, string name, BindingFlags bindingFlags, int genericArgumentCount, int? parametersCount)
        {
            return type.GetMethods(bindingFlags)
                .FirstOrDefault(x => 
                    x.Name == name && 
                    x.IsGenericMethod &&
                    (!parametersCount.HasValue || x.GetParameters().Count() == parametersCount.Value) &&
                    x.GetGenericArguments().Count() == genericArgumentCount);
        }
    }
}
