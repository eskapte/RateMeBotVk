using Microsoft.EntityFrameworkCore;
using RateMeBotVk.DataAccess;
using RateMeBotVk.Models.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;

namespace RateMeBotVk.Services;

public interface IVkSearchInfoService
{
    public Task<AbstractUser> SearchUserByUsernameAsync(string username, CancellationToken token = default);
}

public class VkSearchInfoService : IVkSearchInfoService
{
    private readonly IVkApi _vkApi;

    public VkSearchInfoService(IVkApi vkApi)
    {
        _vkApi = vkApi;
    }

    public async Task<AbstractUser> SearchUserByUsernameAsync(string username, CancellationToken token = default)
    {
        var vkUsers = await _vkApi.Users.GetAsync(new[] { username }, ProfileFields.Domain, NameCase.Gen);
        var vkUser = vkUsers.FirstOrDefault();

        if (vkUser is null)
            return new NotFoundUser(username);

        var fullName = $"{vkUser.FirstName} {vkUser.LastName}";

        //if (dbUser is not null)
        //{
        //    return new UserWithRating(username)
        //    {
        //        RatesCount = dbUser.RatesCount,
        //        Rating = dbUser.Rating,
        //        FullName = fullName
        //    };
        //}

        return new UserWithoutRating(vkUser.Domain)
        {
            FullName = fullName,
        };
    }
}
