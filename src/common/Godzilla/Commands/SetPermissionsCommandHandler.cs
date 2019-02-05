using Godzilla.Abstractions.Services;
using Godzilla.Collections.Internal;
using Godzilla.DomainModels;
using Godzilla.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class SetPermissionsCommandHandler<TContext> : IRequestHandler<SetPermissionsCommand<TContext>, Unit>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;

        public SetPermissionsCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
        }

        public async Task<Unit> Handle(SetPermissionsCommand<TContext> request, CancellationToken cancellationToken)
        {
            try
            {
                _transactionService.StartTransaction();

                var edgesCollection = _transactionService.GetCollection<EntityNode, EntityNodesCollection>();

                await _commandsHelper.VerifyEntity(request.EntityId, edgesCollection, SecurityRight.Administer);

                var securityRulesCollection = _transactionService.GetCollection<EntitySecurityRule, SecurityRulesCollection>();

                if (request.Rules != null)
                    await securityRulesCollection.SetRules(request.EntityId, request.Subject, request.Rules);
                else
                    await securityRulesCollection.ClearRules(request.EntityId, request.Subject);


                _transactionService.CommitTransaction();
                return Unit.Value;
            }
            catch (Exception e)
            {
                _transactionService.AbortTransaction();
                throw new SetPermissionsException("Error setting permissions", e);
            }
        }
    }
}
