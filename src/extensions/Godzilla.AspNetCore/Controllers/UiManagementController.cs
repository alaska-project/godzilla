using Godzilla.Abstractions.Services;
using Godzilla.AspNetCore.Ui.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [Produces(typeof(IEnumerable<UiEntityContext>))]
        public IActionResult GetContexts()
        {
            var contexts = _contextResolver.GetContextReferences();
            return Ok(contexts);
        }

        [Produces(typeof(IEnumerable<UiNode>))]
        public IActionResult GetRootNodes(string contextId)
        {
            var context = GetContext(contextId);
            return Ok();
        }

        public EntityContext GetContext(string contextId)
        {
            var contextType = _contextResolver.GetContextReference(contextId).ContextType;
            return (EntityContext)_serviceProvider.GetService(contextType);
        }
    }
}
