using ContentGenerator.Models.Authentication;

namespace ContentGenerator.Services;

public interface IUserDbService
{
    public Task<bool> DoesUserExist(string username);
    public Task<bool> DoesUserExist(string username, string hashedPassword);
    public Task User_CreateAsync(AppUser user);
}