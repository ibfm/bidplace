﻿using System.Text.Json.Serialization;
using BidPlace.Basket.API.Repositories;
using BidPlace.Basket.API.IntegrationEvents.EventHandling;
using BidPlace.Basket.API.IntegrationEvents.EventHandling.Events;

namespace BidPlace.Basket.API.Extensions;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultAuthentication();

        builder.AddRedisClient("redis");

        builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

        builder.AddRabbitMqEventBus("eventbus")
               .AddSubscription<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>()
               .ConfigureJsonOptions(options => options.TypeInfoResolverChain.Add(IntegrationEventContext.Default));
    }
}

[JsonSerializable(typeof(OrderStartedIntegrationEvent))]
partial class IntegrationEventContext : JsonSerializerContext
{

}
