using System.Text.Json.Serialization;
using BidPlace.ClientApp.Models.Catalog;
using BidPlace.ClientApp.Models.Orders;
using BidPlace.ClientApp.Models.Token;

namespace BidPlace.ClientApp.Services;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(CancelOrderCommand))]
[JsonSerializable(typeof(CatalogBrand))]
[JsonSerializable(typeof(CatalogItem))]
[JsonSerializable(typeof(CatalogRoot))]
[JsonSerializable(typeof(CatalogType))]
[JsonSerializable(typeof(Models.Orders.Order))]
[JsonSerializable(typeof(Models.Location.Location))]
[JsonSerializable(typeof(UserToken))]
internal partial class EShopJsonSerializerContext : JsonSerializerContext
{
}
