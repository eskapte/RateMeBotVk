using Newtonsoft.Json;
using RateMeBotVk.Helpers;
using RateMeBotVk.Services;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;

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
        var command = JsonConvert.DeserializeObject<Command>(message.Payload);

        switch (command.CommandType)
        {
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

    private async Task ProcessSettingsCommandAsync(Message message, CancellationToken token = default)
    {
        var responseText = ResponseTemplates.AboutSettings(true);
        var msgOptions = new MessagesSendParams
        {
            PeerId = message.PeerId.Value,
            Message = responseText,
            Keyboard = KeyboardHelper.GetSettings()
        };

        await _vkMessageService.SendMessageAsync(msgOptions, token: token);
    }

    private async Task ProcessBackCommandAsync(Message message, CancellationToken token = default)
    {
        var responseText = ResponseTemplates.Back;
        var msgOptions = new MessagesSendParams
        {
            PeerId = message.PeerId.Value,
            Message = responseText,
            Keyboard = KeyboardHelper.GetMain()
        };

        await _vkMessageService.SendMessageAsync(msgOptions, token: token);
    }

    private async Task ProcessUnknowCommandAsync(Message message, CancellationToken token = default)
    {
        var msgOptions = new MessagesSendParams
        {
            PeerId = message.PeerId.Value,
            Message = ResponseTemplates.CommandNotFound,
        };

        await _vkMessageService.SendMessageAsync(msgOptions, token: token);
    }

    #endregion CommandsProcessors

    public void Execute(Message message)
    {
        ExecuteAsync(message).RunSynchronously();
    }
}
