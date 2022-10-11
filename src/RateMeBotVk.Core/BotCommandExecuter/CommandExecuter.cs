using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RateMeBotVk.Extensions;
using RateMeBotVk.Helpers;
using RateMeBotVk.Models.Users;
using RateMeBotVk.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Model;

namespace RateMeBotVk.BotCommandExecuter;

public interface ICommandExecuter
{
    public Task ExecuteAsync(Message message, CancellationToken token = default);
    public void Execute(Message message);
}

public class CommandExecuter : ICommandExecuter
{
    private readonly IVkMessageService _vkMessageService;
    private readonly IVkSearchInfoService _vkSearchInfoService;
    private readonly ILogger<CommandExecuter> _logger;

    public CommandExecuter(
        IVkMessageService vkMessageService, 
        IVkSearchInfoService vkSearchInfoService, 
        ILogger<CommandExecuter> logger)
    {
        _vkMessageService = vkMessageService;
        _vkSearchInfoService = vkSearchInfoService;
        _logger = logger;
    }

    public async Task ExecuteAsync(Message message, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        Command command;
        if (string.IsNullOrEmpty(message.Payload))
        {
            command = new Command(CommandType.Search);
        }
        else
        {
            command = JsonConvert.DeserializeObject<Command>(message.Payload);
        }

        switch (command.CommandType)
        {
            case CommandType.Search:
                await ProcessSearchCommand(message, token);
                break;
            case CommandType.Rate:
                await ProcessRateCommandAsync(message, token);
                break;
            case CommandType.Settings:
                await ProcessSettingsCommandAsync(message, token);
                break;
            case CommandType.Back:
                await ProcessBackCommandAsync(message, token);
                break;
            default:
                await ProcessUnknowCommandAsync(message, token);
                break;
        }
    }

    #region CommandsProcessors

    private async Task ProcessSearchCommand(Message message, CancellationToken token = default)
    {
        var username = GetUsernameFromUrl(message.Text);
        var userData = await _vkSearchInfoService.SearchUserByUsernameAsync(username, token);

        var response = userData switch
        {
            UserWithoutRating user => ResponseHelper
                .UserWithoutRating(user.FullName, user.Username)
                .ToPeer(message.PeerId.Value),

            UserWithRating user => ResponseHelper
                .UserProfile(user.FullName, user.Username, user.Rating, user.RatesCount)
                .ToPeer(message.PeerId.Value),

            NotFoundUser _ => ResponseHelper.NotFoundUser,

            _ => throw new NotImplementedException("Undefined User type")
        };

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    private async Task ProcessRateCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.SetRateValue().ToPeer(message.PeerId.Value);

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    private async Task ProcessSettingsCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.Settings(true).ToPeer(message.PeerId.Value);

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    private async Task ProcessBackCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.Back.ToPeer(message.PeerId.Value);

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    private async Task ProcessUnknowCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.CommandNotFount.ToPeer(message.PeerId.Value);

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    #endregion CommandsProcessors

    public void Execute(Message message)
    {
        ExecuteAsync(message).RunSynchronously();
    }

    private string GetUsernameFromUrl(string message)
    {
        var pattern = @"vk\.com\/(?<username>[\w-]{5,32})$";
        var regex = new Regex(pattern);
        var match = regex.Match(message);

        return match.Success ? match.Result("${username}") : message;
    }
}
