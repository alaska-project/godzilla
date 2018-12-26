using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Godzilla.Mongo.Infrastructure
{
    /// <summary>
    /// A convention that allows you to set the serialization representation of guid to a simple string
    /// </summary>
    public class GuidAsStringRepresentationConvention : ConventionBase, IMemberMapConvention
    {
        private List<Assembly> _protectedAssemblies;

        // constructors
        /// <summary>
        /// Initializes a new instance of the  class.
        /// </summary>  
        public GuidAsStringRepresentationConvention(List<Assembly> protectedAssemblies)
        {
            this._protectedAssemblies = protectedAssemblies;
        }

        /// <summary>
        /// Applies a modification to the member map.
        /// </summary>
        /// The member map.
        public void Apply(BsonMemberMap memberMap)
        {
            var memberTypeInfo = memberMap.MemberType;
            if (memberTypeInfo == typeof(Guid))
            {
                var declaringTypeAssembly = memberMap.ClassMap.ClassType.AssemblyQualifiedName;
                var asmName = declaringTypeAssembly;
                if (_protectedAssemblies.Any(a => a.FullName.Equals(asmName, StringComparison.OrdinalIgnoreCase)))
                {
                    return;
                }

                var serializer = memberMap.GetSerializer();
                if (serializer is IRepresentationConfigurable representationConfigurableSerializer)
                {
                    var representation = BsonType.String;
                    var reconfiguredSerializer = representationConfigurableSerializer.WithRepresentation(representation);
                    memberMap.SetSerializer(reconfiguredSerializer);
                }
            }
        }
    }
}
