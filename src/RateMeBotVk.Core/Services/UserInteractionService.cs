using Microsoft.EntityFrameworkCore;
using RateMeBotVk.Core.Models;
using RateMeBotVk.DataAccess;
using System.Threading;
using System.Threading.Tasks;

namespace RateMeBotVk.Core.Services;

public interface IUserInteractionService
{
    public Task<bool> SetRateAsync(UserProfile editableUser, long ownerId, CancellationToken token = default);
}

public class UserInteractionService : IUserInteractionService
{
    private readonly Db _db;

    public UserInteractionService(Db db)
    {
        _db = db;
    }

    public async Task<bool> SetRateAsync(UserProfile editableUser, long ownerId, CancellationToken token = default)
    {
        var dbUser = await _db.Users.AsNoTracking().SingleAsync(x => x.Id == editableUser.UserId);

        return true;
    }
}
