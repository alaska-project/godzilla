using Godzilla.Abstractions;
using Godzilla.Abstractions.Services;
using Godzilla.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Services
{
    internal class EntityCommands<TContext> : IEntityCommands
        where TContext : EntityContext
    {
        #region Init

        private readonly IEntityPropertyResolver<TContext> _propertyResolver;
        private readonly IMediator _mediator;

        private readonly ICollectionService<TContext> _collectionService;
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _entityCommandsHelper;

        public EntityCommands(
            IMediator mediator,
            IEntityPropertyResolver<TContext> propertyResolver,
            ITransactionService<TContext> transactionService,
            ICollectionService<TContext> collectionService,
            IEntityCommandsHelper<TContext> entityCommandsHelper
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _collectionService = collectionService ?? throw new ArgumentNullException(nameof(collectionService));
            _entityCommandsHelper = entityCommandsHelper ?? throw new ArgumentNullException(nameof(entityCommandsHelper));
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

        public async Task<TEntity> Update<TEntity, TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value)
        {
            var id = _propertyResolver.GetEntityId(entity);

            return await Update<TEntity, TField>(id, field, value);
        }

        public async Task<IEnumerable<TEntity>> Update<TEntity, TField>(IEnumerable<TEntity> entities, Expression<Func<TEntity, TField>> field, TField value)
        {
            var idList = entities.Select(x => _propertyResolver.GetEntityId(x));

            return await Update<TEntity, TField>(idList, field, value);
        }

        public async Task<TEntity> Update<TEntity, TField>(Guid entityId, Expression<Func<TEntity, TField>> field, TField value)
        {
            var results = await Update(new List<Guid> { entityId }, field, value);

            return results.First();
        }

        public async Task<IEnumerable<TEntity>> Update<TEntity, TField>(IEnumerable<Guid> idList, Expression<Func<TEntity, TField>> field, TField value)
        {
            //TODO: mediatr generics
            //var result =
            //    await _mediator.Send(new PartialUpdateEntitiesCommand<TContext, TEntity, TField>(idList, field, value));

            var request = new PartialUpdateEntitiesCommand<TContext, TEntity, TField>(idList, field, value);
            var handler = new PartialUpdateEntitiesCommandHandler<TContext, TEntity, TField>(_transactionService, _entityCommandsHelper);
            var result = await handler.Handle(request, default(CancellationToken));

            return result
                .ToList();
        }

        #endregion

        #region Delete

        public async Task Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
        {
            var request = new DeleteFilteredEntitiesCommand<TContext, TEntity>(filter);
            var handler = new DeleteFilteredEntitiesCommandHandler<TContext, TEntity>(_mediator, _collectionService);
            await handler.Handle(request, default(CancellationToken));
        }

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

        public async Task Move<TEntity>(TEntity entity, Guid newParentId)
        {
            var entityId = _propertyResolver.GetEntityId(entity);
            await Move(entityId, newParentId);
        }

        public async Task Move<TEntity>(TEntity entity, object newParent)
        {
            var entityId = _propertyResolver.GetEntityId(entity);
            var newParentId = _propertyResolver.GetEntityId(newParent);
            await Move(entityId, newParentId);
        }
        
        public async Task Move(Guid entityId, Guid newParentId)
        {
            await _mediator.Send(new MoveEntityCommand<TContext>(entityId, newParentId));
        }

        #endregion

        #region Rename

        public async Task Rename<TEntity>(TEntity entity, string newName)
        {
            var entityId = _propertyResolver.GetEntityId(entity);
            await Rename(entityId, newName);
        }

        public async Task Rename(Guid entityId, string newName)
        {
            await _mediator.Send(new RenameEntityCommand<TContext>(entityId, newName));
        }

        #endregion
    }
}
