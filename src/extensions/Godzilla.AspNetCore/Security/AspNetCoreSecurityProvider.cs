using Godzilla.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Godzilla.AspNetCore.Security
{
    public class AspNetCoreSecurityProvider<TContext> : ISecurityContextProvider<TContext>
        where TContext : EntityContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AspNetCoreSecurityProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public bool IsAuthenticated => _httpContextAccessor
            .HttpContext
            .User
            .Identity
            .IsAuthenticated;

        public string UserId => _httpContextAccessor
            .HttpContext
            .User
            .FindFirst(ClaimTypes.NameIdentifier)
            .Value;

        public IEnumerable<string> GetRoles() => _httpContextAccessor
            .HttpContext
            .User
            .FindAll(ClaimTypes.Role)
            .Select(x => x.Value)
            .ToList();
    }
}
