using ContentGenerator.Models.Authentication;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace ContentGenerator.Services;

public interface IUserService
{
    public Task<bool> IsUserNameFree(string username);
    public Task CreateUser(AppUser user, string password);
    public Task<bool> CheckPassword(string username, string password);
}