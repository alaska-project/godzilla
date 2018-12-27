using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
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
        #region Init

        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IMediator _mediator;

        public EntityCommands(
            IMediator mediator,
            IEntityPropertyResolver<TContext> propertyResolver)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
        } 

        #endregion

        #region Add

        public async Task<TEntity> Add<TEntity>(TEntity entity)
        {
            var result = await Add((IEnumerable<TEntity>)new List<TEntity> { entity });
            return result.First();
        }

        public async Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities)
        {
            var command = new CreateEntitiesCommand<TContext>(Guid.Empty, entities.Cast<object>());
            return await SendCreateEntitiesCommand<TEntity>(command);
        }

        public async Task<TEntity> Add<TEntity>(TEntity entity, Guid parentId)
        {
            var result = await Add((IEnumerable<TEntity>)new List<TEntity> { entity }, parentId);
            return result.First();
        }

        public async Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities, Guid parentId)
        {
            var command = new CreateEntitiesCommand<TContext>(parentId, entities.Cast<object>());
            return await SendCreateEntitiesCommand<TEntity>(command);
        }

        public async Task<TEntity> Add<TEntity>(TEntity entity, object parent)
        {
            var result = await Add((IEnumerable<TEntity>)new List<TEntity> { entity }, parent);
            return result.First();
        }

        public async Task<IEnumerable<TEntity>> Add<TEntity>(IEnumerable<TEntity> entities, object parent)
        {
            var parentId = _propertyResolver.GetEntityId(parent);
            return await Add(entities, parentId);
        }

        private async Task<IEnumerable<TEntity>> SendCreateEntitiesCommand<TEntity>(CreateEntitiesCommand<TContext> command)
        {
            var result =
                await _mediator.Send(command);

            return result
                .Cast<TEntity>()
                .ToList();
        }

        #endregion

        #region Update

        public async Task<TEntity> Update<TEntity>(TEntity entity)
        {
            var result = await Update((IEnumerable<TEntity>)new List<TEntity> { entity });
            return result.First();
        }

        public async Task<IEnumerable<TEntity>> Update<TEntity>(IEnumerable<TEntity> entities)
        {
            var result =
                await _mediator.Send(new UpdateEntitiesCommand<TContext>(entities.Cast<object>()));

            return result
                .Cast<TEntity>()
                .ToList();
        }

        #endregion

        #region Delete

        public async Task Delete<TEntity>(TEntity entity)
        {
            await Delete((IEnumerable<TEntity>)new List<TEntity> { entity });
        }

        public async Task Delete<TEntity>(IEnumerable<TEntity> entities)
        {
            await _mediator.Send(new DeleteEntitiesCommand<TContext>(entities.Cast<object>()));
        }

        #endregion

        #region Move

        public Task Move<TEntity>(TEntity entity, Guid newParentId)
        {
            throw new NotImplementedException();
        }

        public Task Move<TEntity>(TEntity entity, object newParent)
        {
            throw new NotImplementedException();
        }

        public Task Move<TEntity>(IEnumerable<TEntity> entities, Guid newParentId)
        {
            throw new NotImplementedException();
        }

        public Task Move<TEntity>(IEnumerable<TEntity> entities, object newParent)
        {
            throw new NotImplementedException();
        }

        public Task Move(Guid entityId, Guid newParentId)
        {
            throw new NotImplementedException();
        }

        public Task Move(IEnumerable<Guid> entitiesId, Guid newParentId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Rename

        public Task Rename<TEntity>(TEntity entity, string newName)
        {
            throw new NotImplementedException();
        }

        public Task Rename(Guid entityId, string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
