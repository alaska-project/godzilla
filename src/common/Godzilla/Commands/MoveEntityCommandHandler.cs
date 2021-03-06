﻿using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class MoveEntityCommandHandler<TContext> : IRequestHandler<MoveEntityCommand<TContext>, Unit>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;
        private readonly IPathBuilder<TContext> _pathBuilder;

        public MoveEntityCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper,
            IPathBuilder<TContext> pathBuilder)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
        }

        public async Task<Unit> Handle(MoveEntityCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                _transactionService.StartTransaction();

                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                var moveTargetNode = await _commandsHelper.VerifyEntity(request.NewParentId, edgesCollection, SecurityRight.Create);
                if (moveTargetNode == null)
                    throw new NodeNotFoundException($"Target node {request.NewParentId} not found");

                var moveSourceNode = await _commandsHelper.VerifyEntity(request.EntityId, edgesCollection, SecurityRight.Move);
                if (moveSourceNode == null)
                    throw new NodeNotFoundException($"Source node {request.EntityId} not found");

                var nodesToMove = edgesCollection.GetDescendants(moveSourceNode)
                    .ToList();

                MoveNodes(nodesToMove, moveSourceNode, moveTargetNode);

                await edgesCollection.Update(nodesToMove);

                _transactionService.CommitTransaction();
                return Unit.Value;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntityMoveException("Entity move failed", e);
            }
        }

        private void MoveNodes(IEnumerable<EntityNode> nodesToMove, EntityNode source, EntityNode target)
        {
            foreach(var node in nodesToMove)
            {
                if (node.ParentId == source.ParentId)
                    node.ParentId = target.ParentId;

                node.Path = _pathBuilder.MovePath(node.Path, source.Path, target.Path);
                node.IdPath = _pathBuilder.MovePath(node.IdPath, source.IdPath, target.IdPath);
            }
        }
    }
}
