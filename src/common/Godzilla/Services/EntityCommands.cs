﻿using Godzilla.Abstractions;
using Godzilla.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Services
{
    internal class EntityCommands<TContext> : IEntityCommands
        where TContext : EntityContext
    {
        private readonly IMediator _mediator;

        public EntityCommands(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TEntity> Add<TEntity>(TEntity entity)
        {
            var result = await Add((IEnumerable<TEntity>)new List<TEntity> { entity });
            return result.First();
        }

        public async Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities)
        {
            var result = 
                await _mediator.Send(new CreateEntitiesCommand<TContext>(Guid.Empty, entities.Cast<object>()));

            return result
                .Cast<TEntity>()
                .ToList();

        }
    }
}