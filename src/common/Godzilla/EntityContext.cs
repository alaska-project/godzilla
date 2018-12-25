using Godzilla.Abstractions.Services;
using Godzilla.Commands;
using Godzilla.Services;
using MediatR;
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
    public abstract class EntityContext
    {
        private readonly IMediator _mediator;

        public EntityContext(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Commands = (IEntityCommandRunner)Activator.CreateInstance(typeof(EntityCommandRunner<>).MakeGenericType(this.GetType()), new object[] { mediator });
        }

        public IEntityCommandRunner Commands { get; }
        
        public virtual void OnConfiguring()
        { }
    }
}
