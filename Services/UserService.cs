using ContentGenerator.Models.Authentication;

namespace ContentGenerator.Services;

public class UserService : IUserService
{
    private readonly IUserDbService _userDbService;
    public UserService(IUserDbService userDbService)
    {
        _userDbService = userDbService;
    }

    public async Task<bool> CheckPassword(string username, string password)
    {
        return await _userDbService.DoesUserExist(username, HashPass(password));
    }

    public async Task CreateUser(AppUser user, string password)
    {
        user.PasswordHash = HashPass(password);
        await _userDbService.User_CreateAsync(user);
    }

    public async Task<bool> IsUserNameFree(string username)
    {
        return !await _userDbService.DoesUserExist(username);
    }

    private string HashPass(string password)
    {
        return password.ToUpper();
    }
}