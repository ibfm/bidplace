﻿namespace BidPlace.Ordering.API.Application.Commands;

public record ShipOrderCommand(int OrderNumber) : IRequest<bool>;
