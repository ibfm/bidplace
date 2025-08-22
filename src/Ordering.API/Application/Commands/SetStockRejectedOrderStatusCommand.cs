﻿namespace BidPlace.Ordering.API.Application.Commands;

public record SetStockRejectedOrderStatusCommand(int OrderNumber, List<int> OrderStockItems) : IRequest<bool>;
