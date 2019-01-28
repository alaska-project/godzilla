using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("MediatR")]
[assembly: InternalsVisibleTo("MediatR.Extensions.Microsoft.DependencyInjection")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Godzilla.UnitTests")]
namespace Godzilla
{
    public abstract class EntityContext<TContext> : EntityContext
        where TContext : EntityContext
    {
        public EntityContext(IEntityContextServices<TContext> services)
            : base(services)
        { }
    }

    public abstract class EntityContext
    {
        private IEntityContextServices _entityContextServices;
        private IDocumentService _documentQueries;

        internal EntityContext(IEntityContextServices entityContextServices)
        {
            _documentQueries = new DocumentService(
                this, 
                entityContextServices.Queries,
                entityContextServices.Commands);

            _entityContextServices = entityContextServices;

            entityContextServices.Initializer.Initialize(this, entityContextServices.Configurator);
        }

        public IEntityCommands Commands => _entityContextServices.Commands;
        public IEntityQueries Query => _entityContextServices.Queries;
        public IDocumentService Documents => _documentQueries;

        protected IEntityConfigurator Configurator => _entityContextServices.Configurator;

        public virtual void OnConfiguring()
        { }
    }
}
