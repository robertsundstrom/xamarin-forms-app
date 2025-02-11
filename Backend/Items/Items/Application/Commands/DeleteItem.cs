﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ShellApp.Items.Application.Common.Interfaces;
using ShellApp.Items.Commands;
using ShellApp.Items.Events;
using ShellApp.Domain.Exceptions;

namespace ShellApp.Items.Application.Commands
{

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteItemCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FindAsync(request.ItemId);

            if (item == null)
            {
                throw new NotFoundException();
            }

            item.DomainEvents.Add(new ItemDeletedEvent(item.Id));

            context.Items.Remove(item);

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
