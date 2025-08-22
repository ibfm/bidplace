using BidPlace.ClientApp.Models.Basket;
using BidPlace.ClientApp.Models.Catalog;
using BidPlace.ClientApp.Models.Marketing;

namespace BidPlace.ClientApp.Services.FixUri;

public interface IFixUriService
{
    void FixCatalogItemPictureUri(IEnumerable<CatalogItem> catalogItems);
    void FixBasketItemPictureUri(IEnumerable<BasketItem> basketItems);
    void FixCampaignItemPictureUri(IEnumerable<CampaignItem> campaignItems);
}
