﻿using Godzilla.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla.Commands
{
    internal class MoveEntitiesCommandHandler<TContext> : IRequestHandler<MoveEntitiesCommand<TContext>, Unit>
        where TContext : EntityContext
    {
        private readonly ITransactionService<TContext> _transactionService;
        private readonly IEntityCommandsHelper<TContext> _commandsHelper;

        public MoveEntitiesCommandHandler(
            ITransactionService<TContext> transactionService,
            IEntityCommandsHelper<TContext> commandsHelper)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _commandsHelper = commandsHelper ?? throw new ArgumentNullException(nameof(commandsHelper));
        }

        public Task<Unit> Handle(MoveEntitiesCommand<TContext> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
