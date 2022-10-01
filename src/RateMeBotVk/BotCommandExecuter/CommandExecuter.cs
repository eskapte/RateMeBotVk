using Newtonsoft.Json;
using RateMeBotVk.Helpers;
using RateMeBotVk.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace RateMeBotVk.BotCommandExecuter;

public interface ICommandExecuter
{
    public Task ExecuteAsync(Message message, CancellationToken token = default);
    public void Execute(Message message);
}

public class CommandExecuter : ICommandExecuter
{
    private readonly IVkMessageService _vkMessageService;

    public CommandExecuter(IVkMessageService vkMessageService)
    {
        _vkMessageService = vkMessageService;
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

    private async Task ProcessSearchCommand(Message message, CancellationToken token)
    {
        await ProcessUnknowCommandAsync(message, token);
    }

    private async Task ProcessSettingsCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.Settings(true);
        response.PeerId = message.PeerId.Value;

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    private async Task ProcessBackCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.Back;
        response.PeerId = message.PeerId.Value;

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    private async Task ProcessUnknowCommandAsync(Message message, CancellationToken token = default)
    {
        var response = ResponseHelper.CommandNotFount;
        response.PeerId = message.PeerId.Value;

        await _vkMessageService.SendMessageAsync(response, token: token);
    }

    #endregion CommandsProcessors

    public void Execute(Message message)
    {
        ExecuteAsync(message).RunSynchronously();
    }
}
