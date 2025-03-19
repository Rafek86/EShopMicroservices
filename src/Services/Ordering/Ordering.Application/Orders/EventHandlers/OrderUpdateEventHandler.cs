using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderUpdateEventHandler(ILogger<OrderUpdateEventHandler> logger)
          : INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled : {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
