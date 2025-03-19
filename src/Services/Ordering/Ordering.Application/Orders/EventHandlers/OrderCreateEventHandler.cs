namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreateEventHandler(ILogger<OrderCreateEventHandler> logger)
        : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled : {DomainEvent}",notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
