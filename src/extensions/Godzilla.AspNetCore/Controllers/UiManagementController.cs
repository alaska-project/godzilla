using Godzilla.Abstractions.Services;
using Godzilla.AspNetCore.Ui.Model;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.AspNetCore.Controllers
{
    [Route("godzilla/api/management")]
    public class UiManagementController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEntityContextResolver _contextResolver;

        public UiManagementController(IEntityContextResolver contextResolver, IServiceProvider serviceProvider)
        {
            _contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [Produces(typeof(IEnumerable<UiEntityContextReference>))]
        public IActionResult GetContexts()
        {
            var contexts = _contextResolver.GetContextReferences();
            return Ok(contexts);
        }

        [Produces(typeof(IEnumerable<UiNodeReference>))]
        public IActionResult GetRootNodes(string contextId)
        {
            var nodesCollection = GetEntityNodesCollection(contextId);
            var nodes = nodesCollection
                .AsQueryable()
                .Where(x => x.ParentId == Guid.Empty)
                .Select(x => new { x.EntityId, x.NodeName, x.ParentId })
                .ToList();

            var convertedNodes = nodes
                .Select(x => new UiNodeReference
                {
                    Id = x.EntityId,
                    Name = x.NodeName,
                    ParentId = x.ParentId,
                })
                .ToList();

            return Ok(convertedNodes);
        }

        private EntityNodesCollection GetEntityNodesCollection(string contextId)
        {
            return GetContext(contextId).Collections.GetCollection<EntityNode, EntityNodesCollection>();
        }

        private EntityContext GetContext(string contextId)
        {
            var contextType = _contextResolver.GetContextReference(contextId).ContextType;
            return (EntityContext)_serviceProvider.GetService(contextType);
        }
    }
}
