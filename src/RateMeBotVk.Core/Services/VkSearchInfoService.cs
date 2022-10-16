﻿using Microsoft.EntityFrameworkCore;
using RateMeBotVk.Core.Models;
using RateMeBotVk.DataAccess;
using RateMeBotVk.DataAccess.Models;
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
    private readonly Db _db;

    public VkSearchInfoService(IVkApi vkApi, Db db)
    {
        _vkApi = vkApi;
        _db = db;
    }

    public async Task<AbstractUser> SearchUserByUsernameAsync(string username, CancellationToken token = default)
    {
        var vkUsers = await _vkApi.Users.GetAsync(new[] { username }, ProfileFields.Domain, NameCase.Gen);
        var vkUser = vkUsers.FirstOrDefault();

        if (vkUser is null)
            return new NotFoundUser();

        var dbUser = await _db.Users
            .Include(x => x.GotRates)
            .FirstOrDefaultAsync(x => x.Id == vkUser.Id, token);

        if (dbUser is null)
        {
            dbUser = new User
            {
                Username = vkUser.Domain
            };
            await _db.Users.AddAsync(dbUser, token);
            await _db.SaveChangesAsync(token);
        }

        if (dbUser.Username != vkUser.Domain)
        {
            dbUser.Username = vkUser.Domain;
            await _db.SaveChangesAsync(token);
        }

        var likes = dbUser.GotRates.Count(x => x.IsLike);
        var dislikes = dbUser.GotRates.Count(x => !x.IsLike);
        var total = likes + dislikes;

        var fullName = $"{vkUser.FirstName} {vkUser.LastName}";

        return new UserProfile(vkUser.Domain, fullName, vkUser.Id, likes, dislikes, total);
    }
}
