using Godzilla.DomainModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Commands
{
    internal class SetPermissionsCommand<TContext> : IRequest<Unit>
        where TContext : EntityContext
    {
        public SetPermissionsCommand(Guid entityId, RuleSubject subject, IEnumerable<SecurityRule> rules)
        {
            EntityId = entityId;
            Subject = subject;
            Rules = rules;
        }

        public Guid EntityId { get; }
        public RuleSubject Subject { get; }
        public IEnumerable<SecurityRule> Rules { get; }
    }
}
