using BidPlace.WebAppComponents.Services;

namespace BidPlace.HybridApp.Services;

public class ProductImageUrlProvider : IProductImageUrlProvider
{
    public string GetProductImageUrl(int productId)
        => $"{MauiProgram.MobileBffHost}api/catalog/items/{productId}/pic?api-version=2.0";
}
