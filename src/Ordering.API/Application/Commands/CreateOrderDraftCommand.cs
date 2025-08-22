namespace BidPlace.Ordering.API.Application.Commands;
using BidPlace.Ordering.API.Application.Models;

public record CreateOrderDraftCommand(string BuyerId, IEnumerable<BasketItem> Items) : IRequest<OrderDraftDTO>;
