using BidPlace.ClientApp.Services.Basket;
using BidPlace.ClientApp.Services.Catalog;
using BidPlace.ClientApp.Services.Identity;
using BidPlace.ClientApp.Services.Order;

namespace BidPlace.ClientApp.Services.AppEnvironment;

public interface IAppEnvironmentService
{
    IBasketService BasketService { get; }

    ICatalogService CatalogService { get; }

    IOrderService OrderService { get; }

    IIdentityService IdentityService { get; }

    void UpdateDependencies(bool useMockServices);
}
