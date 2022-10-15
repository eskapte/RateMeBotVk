using Newtonsoft.Json;
using RateMeBotVk.Core.Models;
using RateMeBotVk.Helpers;
using RateMeBotVk.Models.Users;
using RateMeBotVk.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Core.BotCommandExecuters;

public interface IMessageCommandExecuter
{
    public Task ExecuteAsync(Message message, CancellationToken token = default);
}

public class MessageCommandExecuter : IMessageCommandExecuter
{
    private readonly IVkApi _vkApi;
    private readonly IVkSearchInfoService _vkSearchInfoService;

    public MessageCommandExecuter(IVkApi vkApi, IVkSearchInfoService vkSearchInfoService)
    {
        _vkApi = vkApi;
        _vkSearchInfoService = vkSearchInfoService;
    }

    public async Task ExecuteAsync(Message message, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var command = JsonConvert.DeserializeObject<Payload?>(message.Payload ?? "")?.Command ?? CommandType.Search;

        var response = command switch
        {
            CommandType.Start => ResponseHelper.Start,
            CommandType.Search => await SearhUserAsync(message, token),
            _ => ResponseHelper.CommandNotFount
        };
        response.PeerId = message.PeerId;

        await _vkApi.Messages.SendAsync(response);
    }

    #region Bot Commands

    private async Task<MessagesSendParams> SearhUserAsync(Message message, CancellationToken token = default)
    {
        var username = ParseUsernameIfUrl(message.Text);
        var result = await _vkSearchInfoService.SearchUserByUsernameAsync(username, token);

        return result switch
        {
            UserWithoutRating user => ResponseHelper.UserWithoutRating(user.FullName, user.Username),

            UserWithRating user => ResponseHelper
                .UserProfile(user.FullName, user.Username, user.Rating, user.RatesCount),

            NotFoundUser _ => ResponseHelper.NotFoundUser,

            _ => throw new NotImplementedException("Undefined User type")
        };
    }

    #endregion

    #region private methods

    private string ParseUsernameIfUrl(string message)
    {
        var pattern = @"vk\.com\/(?<username>[\w-]{5,32})$";
        var regex = new Regex(pattern);
        var match = regex.Match(message);

        return match.Success ? match.Result("${username}") : message;
    }

    #endregion
}
