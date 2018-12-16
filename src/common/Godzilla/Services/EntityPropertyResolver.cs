using Godzilla.Abstractions.Services;
using Godzilla.Exceptions;
using Godzilla.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Godzilla.Services
{
    public class EntityPropertyResolver<TContext> : 
        IEntityPropertyResolver<TContext>
        where TContext : EntityContext
    {
        private SafeDictionary<Type, PropertyInfo> _idPropertyCache = new SafeDictionary<Type, PropertyInfo>();
        private SafeDictionary<Type, PropertyInfo> _namePropertyCache = new SafeDictionary<Type, PropertyInfo>();

        public Guid GetEntityId(object entity)
        {
            return GetEntityId(entity, false);
        }

        public Guid GetEntityId(object entity, bool generateIfEmpty)
        {
            var idProperty = _idPropertyCache.Retreive(
                entity.GetType(), 
                () => GetIdProperty(entity));

            var value = (Guid)idProperty.GetValue(entity);
            if (value == Guid.Empty && generateIfEmpty)
            {
                value = Guid.NewGuid();
                idProperty.SetValue(entity, value);
            }

            return value;
        }

        public string GetEntityName(object entity)
        {
            var nameProperty = _namePropertyCache.Retreive(
                entity.GetType(),
                () => GetNameProperty(entity));

            if (nameProperty != null)
            {
                var name = nameProperty.GetValue(entity)?
                    .ToString()
                    .Trim();
                if (!string.IsNullOrEmpty(name))
                    return name;
            }

            var id = GetEntityId(entity);
            if (id == Guid.Empty)
                throw new MissingIdException();

            return id.ToString();
        }

        private PropertyInfo GetIdProperty(object entity)
        {
            var entityType = entity.GetType();
            var idProperty = entityType.GetProperty("Id");
            if (idProperty != null)
            {
                if (idProperty.PropertyType != typeof(Guid))
                    throw new WrongIdPropertyTypeException($"Wrong id property type {idProperty.PropertyType.FullName} for entity {entityType.FullName}. Expected type is Guid");

                return idProperty;
            }
            
            throw new MissingIdPropertyException();
        }

        private PropertyInfo GetNameProperty(object entity)
        {
            var entityType = entity.GetType();
            return entityType.GetProperty("Name");
        }
    }
}
