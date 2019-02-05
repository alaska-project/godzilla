using Godzilla.Security.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla.Abstractions.Services
{
    internal interface ISecurityRuleEvaluator<TContext>
        where TContext : EntityContext
    {
        Task<EvaluateResult> EvaluateRoot(Guid permission);
        Task<IEnumerable<EvaluateResult>> Evaluate(IEnumerable<Guid> entitiesId, Guid permission);
    }
}
