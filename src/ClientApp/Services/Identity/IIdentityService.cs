using BidPlace.ClientApp.Models.User;

namespace BidPlace.ClientApp.Services.Identity;

public interface IIdentityService
{
    Task<bool> SignInAsync();

    Task<bool> SignOutAsync();

    Task<UserInfo> GetUserInfoAsync();

    Task<string> GetAuthTokenAsync();
}
