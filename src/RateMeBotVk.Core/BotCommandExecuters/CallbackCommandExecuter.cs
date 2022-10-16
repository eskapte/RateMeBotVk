using Newtonsoft.Json;
using RateMeBotVk.Core.Models;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.GroupUpdate;

namespace RateMeBotVk.Core.BotCommandExecuters;

public interface ICallbackCommandExecuter
{
    public Task ExecuteAsync(MessageEvent msgEvent, CancellationToken token = default);
}

public class CallbackCommandExecuter : ICallbackCommandExecuter
{
    private readonly IVkApi _vkApi;

    public CallbackCommandExecuter(IVkApi vkApi)
    {
        _vkApi = vkApi;
    }

    public async Task ExecuteAsync(MessageEvent msgEvent, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var payload = JsonConvert.DeserializeObject<Payload?>(msgEvent.Payload ?? "");
        var command = payload?.Command;

        if (command is null)
            return;

        switch (command)
        {
            case CommandType.Rate:
                await RateUserAsync(msgEvent, token);
                break;
            default:
                return;
        }
    }

    private async Task RateUserAsync(MessageEvent msgEvent, CancellationToken token = default)
    {

    }
}
