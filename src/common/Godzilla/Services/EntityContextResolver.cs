using Godzilla.Abstractions.Services;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Services
{
    internal class EntityContextResolver : IEntityContextResolver
    {
        private readonly Dictionary<string, EntityContextReference> _references = new Dictionary<string, EntityContextReference>();

        public void RegisterContext<TContext>()
            where TContext : EntityContext
        {
            var context = new EntityContextReference(
                typeof(TContext).FullName,
                typeof(TContext));

            lock (this)
            {
                if (!_references.ContainsKey(context.ContextId))
                    _references.Add(context.ContextId, context);
            }
        }

        public EntityContextReference GetContextReference(string contextId)
        {
            if (!_references.ContainsKey(contextId))
                throw new InvalidOperationException($"Context {contextId} not found");

            return _references[contextId];
        }

        public IEnumerable<EntityContextReference> GetContextReferences()
        {
            return _references.Values;
        }
    }
}
