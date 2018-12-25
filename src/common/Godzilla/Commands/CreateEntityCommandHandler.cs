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
    public class CreateEntityCommandHandler<TContext> : IRequestHandler<CreateEntityCommand<TContext>, IEnumerable<object>>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityPropertyResolver<TContext> _propertyResolver;

        public CreateEntityCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityPropertyResolver<TContext> propertyResolver)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _propertyResolver = propertyResolver ?? throw new ArgumentNullException(nameof(propertyResolver));
        }

        public Task<IEnumerable<object>> Handle(CreateEntityCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                var entityType = GetEntityType(request.Entities);

                var treeEdges = request.Entities
                    .Select(x => CreateTreeEdge(x, request.ParentId))
                    .ToList();
                
                _transactionService.StartTransaction();

                var edgesCollection = _transactionService.GetCollection<TreeEdge, TreeEdgesCollection>();

                ValidateParentId(edgesCollection, request.ParentId);
                ValidateTreeEdges(edgesCollection, treeEdges);

                edgesCollection.Add(treeEdges);

                var entityCollection = _transactionService.GetCollection(entityType);
                entityCollection.Add(request.Entities);

                _transactionService.CommitTransaction();
                return Task.FromResult(request.Entities);
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new EntityCreationException("Entity creation failed", e);
            }
        }

        private void ValidateParentId(TreeEdgesCollection edgesCollection, Guid parentId)
        {
            if (parentId == Guid.Empty)
                return;

            var parent = edgesCollection
                .AsQueryable()
                .FirstOrDefault(x => x.NodeId == parentId);

            if (parent == null)
                throw new ParentNodeNotFoundException($"Parent node {parentId} not found");
        }

        private void ValidateTreeEdges(TreeEdgesCollection edgesCollection, IEnumerable<TreeEdge> treeEdges)
        {
            var newNodesId = treeEdges
                .Select(x => x.NodeId)
                .ToList();

            var existingNodes = edgesCollection.GetNodes(newNodesId);
            if (existingNodes.Any())
                throw new NodeAlreadyExistsException($"Node {string.Join(", ", existingNodes.Select(x => x.NodeId))} already exists");
        }

        private TreeEdge CreateTreeEdge(object entity, Guid parentId)
        {
            var entityId = _propertyResolver.GetEntityId(entity, true);
            var entityName = _propertyResolver.GetEntityName(entity);

            return new TreeEdge
            {
                Id = Guid.NewGuid(),
                NodeId = entityId,
                NodeName = entityName,
                ParentId = parentId,
            };
        }

        private Type GetEntityType(IEnumerable<object> entities)
        {
            var entityTypes = entities
                    .Where(x => x != null)
                    .GroupBy(x => x.GetType());
            if (entityTypes.Count() > 1)
                throw new MultipleEntityTypesNotSupportedException($"Cannot add multiple entity types inside the same operation. Types: {string.Join(", ", entityTypes.Select(x => x.Key.FullName))}");

            return entityTypes.First().Key;
        }
    }
}
