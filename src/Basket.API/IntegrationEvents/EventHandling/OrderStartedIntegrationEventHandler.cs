﻿using BidPlace.Basket.API.Repositories;
using BidPlace.Basket.API.IntegrationEvents.EventHandling.Events;

namespace BidPlace.Basket.API.IntegrationEvents.EventHandling;

public class OrderStartedIntegrationEventHandler(
    IBasketRepository repository,
    ILogger<OrderStartedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderStartedIntegrationEvent>
{
    public async Task Handle(OrderStartedIntegrationEvent @event)
    {
        logger.LogInformation("Handling integration event: {IntegrationEventId} - ({@IntegrationEvent})", @event.Id, @event);

        await repository.DeleteBasketAsync(@event.UserId);
    }
}
