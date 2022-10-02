using Microsoft.EntityFrameworkCore;
using RateMeBotVk.DataAccess;
using RateMeBotVk.Models.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Services;

public interface IVkSearchInfoService
{
    public Task<AbstractUser> SearchUserByUsernameAsync(string username, CancellationToken token = default);
}

public class VkSearchInfoService : IVkSearchInfoService
{
    private readonly Db _db;
    private readonly IVkApi _vkApi;

    public VkSearchInfoService(Db db, IVkApi vkApi)
    {
        _db = db;
        _vkApi = vkApi;
    }

    public async Task<AbstractUser> SearchUserByUsernameAsync(string username, CancellationToken token = default)
    {
        var vkUsers = await _vkApi.Users.GetAsync(new[] { username }, ProfileFields.Domain, NameCase.Gen);
        var vkUser = vkUsers.FirstOrDefault();

        if (vkUser is null)
            return new NotFoundUser(username);

        return new UserWithoutRating(vkUser.Domain)
        {
            Id = vkUser.Id,
            FullName = $"{vkUser.FirstName} {vkUser.LastName}",
            PhotoUrl = vkUser.PhotoMaxOrig,
        };
    }
}
