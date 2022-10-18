using Newtonsoft.Json;
using MudaBot.Core.Models;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.GroupUpdate;

namespace MudaBot.Core.BotCommandExecuters;

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
            case CommandType.Dislike:
                await DislikeUserAsync(msgEvent, token);
                break;
            default:
                return;
        }
    }

    private async Task DislikeUserAsync(MessageEvent msgEvent, CancellationToken token = default)
    {

    }
}
