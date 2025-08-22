namespace BidPlace.OrderProcessor.Events
{
    using BidPlace.EventBus.Events;

    public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; }

        public GracePeriodConfirmedIntegrationEvent(int orderId) =>
            OrderId = orderId;
    }
}
