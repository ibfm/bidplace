using BidPlace.WebAppComponents.Catalog;

namespace BidPlace.WebAppComponents.Item;

public static class ItemHelper
{
    public static string Url(CatalogItem item)
        => $"item/{item.Id}";
}
