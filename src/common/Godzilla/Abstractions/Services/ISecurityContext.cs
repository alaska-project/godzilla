using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Services
{
    internal interface ISecurityContext<TContext>
        where TContext : EntityContext
    {
        bool IsAdministrator();
        IEnumerable<RuleSubject> GetApplyableSubjects();
    }
}
