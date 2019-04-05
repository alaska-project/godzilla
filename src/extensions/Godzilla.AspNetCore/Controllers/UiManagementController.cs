using Godzilla.Abstractions.Services;
using Godzilla.AspNetCore.Ui.Model;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.AspNetCore.Controllers
{
    [Route("godzilla/api/[controller]/[action]")]
    public class UiManagementController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEntityContextResolver _contextResolver;

        public UiManagementController(IEntityContextResolver contextResolver, IServiceProvider serviceProvider)
        {
            _contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<UiEntityContextReference>))]
        public IActionResult GetContexts()
        {
            var contexts = _contextResolver.GetContextReferences();
            var convertedContexts = contexts
                .Select(x => new UiEntityContextReference
                {
                    Id = x.ContextId,
                    Name = x.ContextType.Name
                })
                .ToList();

            return Ok(convertedContexts);
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<UiNodeReference>))]
        public IActionResult GetRootNodes([FromQuery]string contextId)
        {
            return GetChildNodes(contextId, Guid.Empty);
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<UiNodeReference>))]
        public IActionResult GetChildNodes([FromQuery]string contextId, [FromQuery]Guid parentId)
        {
            var nodesCollection = GetEntityNodesCollection(contextId);
            var nodes = nodesCollection
                .AsQueryable()
                .Where(x => x.ParentId == parentId)
                .Select(x => new { x.EntityId, x.NodeName, x.ParentId })
                .ToList();

            var nodesId = nodes
                .Select(x => x.EntityId)
                .ToList();

            var nodesIdWithChildren = nodesCollection
                .AsQueryable()
                .Where(x => nodesId.Contains(x.ParentId))
                .GroupBy(x => x.ParentId)
                .Select(x => x.First().ParentId)
                .ToList();

            var convertedNodes = nodes
                .Select(x => new UiNodeReference
                {
                    Id = x.EntityId,
                    Name = x.NodeName,
                    ParentId = x.ParentId,
                    IsLeaf = !nodesIdWithChildren.Contains(x.EntityId),
                })
                .ToList();

            return Ok(convertedNodes);
        }

        [HttpGet]
        [Produces(typeof(UiNodeValue))]
        public async Task<IActionResult> GetNode([FromQuery]string contextId, [FromQuery]Guid nodeId)
        {
            var nodesCollection = GetEntityNodesCollection(contextId);
            var node = nodesCollection.GetNode(nodeId);
            if (node == null)
                return NotFound();

            var collection = GetCollectionService(contextId).GetRawCollection(node.CollectionId);
            var item = await collection.GetRawItem(nodeId);

            return Ok(new UiNodeValue
            {
                Id = nodeId,
                SerializedValue = item
            });
        }

        private EntityNodesCollection GetEntityNodesCollection(string contextId)
        {
            return GetContext(contextId).Collections.GetCollection<EntityNode, EntityNodesCollection>();
        }

        private ICollectionService GetCollectionService(string contextId)
        {
            var context = GetContext(contextId);
            return GetContextService<ICollectionService>(contextId, typeof(ICollectionService<>));
        }

        private TService GetContextService<TService>(string contextId, Type serviceGenericType)
        {
            var contextType = _contextResolver.GetContextReference(contextId).ContextType;
            var serviceImplementedType = serviceGenericType.MakeGenericType(new Type[] { contextType });
            return (TService)_serviceProvider.GetService(serviceImplementedType);
        }

        private EntityContext GetContext(string contextId)
        {
            var contextType = _contextResolver.GetContextReference(contextId).ContextType;
            return (EntityContext)_serviceProvider.GetService(contextType);
        }
    }
}
