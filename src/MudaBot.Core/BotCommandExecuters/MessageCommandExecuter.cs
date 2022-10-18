using Newtonsoft.Json;
using MudaBot.Core.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using MudaBot.Core.Helpers;
using MudaBot.Core.Services;

namespace MudaBot.Core.BotCommandExecuters;

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

        var payload = JsonConvert.DeserializeObject<Payload?>(message.Payload ?? "");
        var command = payload?.Command ?? CommandType.Search;

        var response = command switch
        {
            CommandType.Start => ResponseHelper.Start,
            CommandType.Search => await SearchUserAsync(message, token),
            CommandType.UserProfile => await SearchUserAsync(message, token),
            _ => ResponseHelper.CommandNotFount
        };
        response.PeerId = message.PeerId;

        await _vkApi.Messages.SendAsync(response);
    }

    #region Bot Commands

    private async Task<MessagesSendParams> SearchUserAsync(Message message, CancellationToken token = default)
    {
        var username = message.Text.StartsWith('@') 
            ? message.Text[1..]
            : ParseUsernameIfUrl(message.Text);
        var result = await _vkSearchInfoService.SearchUserByUsernameAsync(username, token);

        return result switch
        {
            UserProfile user when user.UserId == message.PeerId => ResponseHelper.UserProfile(user),
            UserProfile user => ResponseHelper.FoundUserProfile(user),
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
