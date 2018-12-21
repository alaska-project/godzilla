using Godzilla.Abstractions.Services;
using Godzilla.Commands;
using Godzilla.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Godzilla.UnitTests")]
namespace Godzilla
{
    public abstract class EntityContext
    {
        private readonly IEntityCommandRunner _runner;
        private readonly IMediator _mediator;

        public EntityContext(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _runner = (IEntityCommandRunner)Activator.CreateInstance(typeof(EntityCommandRunner<>).MakeGenericType(this.GetType()), new object[] { mediator });
        }

        public async Task Add<TEntity>(TEntity entity)
        {
            await _runner.Add<TEntity>(entity);
        }

        public virtual void OnConfiguring()
        { }
    }
}
