using Godzilla.Abstractions.Services;
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
    internal class RenameEntityCommandHandler<TContext> : IRequestHandler<RenameEntityCommand<TContext>, Unit>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;
        private readonly IPathBuilder<TContext> _pathBuilder;

        public RenameEntityCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper,
            IPathBuilder<TContext> pathBuilder)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
            _pathBuilder = pathBuilder ?? throw new ArgumentNullException(nameof(pathBuilder));
        }

        public async Task<Unit> Handle(RenameEntityCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                _transactionService.StartTransaction();

                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                var renameRootNode = await _commandsHelper.VerifyEntity(request.EntityId, edgesCollection, SecurityRight.Rename);
                var renameDescendants = edgesCollection.GetDescendants(renameRootNode)
                    .ToList();

                var oldRootPath = renameRootNode.Path;
                var newRootPath = _pathBuilder.RenameLeaf(oldRootPath, request.NewName);

                // renameRootNode is set with the instance taken from renameDescendants
                renameRootNode = renameDescendants.First(x => x.Id == renameRootNode.Id);
                renameRootNode.NodeName = request.NewName;

                renameDescendants
                    .ForEach(x => UpdateNodePath(x, oldRootPath, newRootPath));

                await edgesCollection.Update(renameDescendants);

                _transactionService.CommitTransaction();
                return Unit.Value;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntitiesRenameException("Entity rename failed", e);
            }
        }

        private void UpdateNodePath(EntityNode node, string oldRootPath, string newRootPath)
        {
            node.Path = newRootPath + node.Path.Substring(oldRootPath.Length);
        }
    }
}
