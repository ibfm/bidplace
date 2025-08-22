namespace BidPlace.ClientApp.Services.Location;

public interface ILocationService
{
    Task UpdateUserLocation(Models.Location.Location newLocReq);
}
