using Microsoft.Extensions.Hosting;
using VkNet.Model;
using Microsoft.Extensions.Logging;
using MoreLinq;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using RateMeBotVk.Services;
using Microsoft.Extensions.Options;

namespace RateMeBotVk;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _setting;
    private readonly IVkMessageService _vkMessageService;

    private readonly TimeSpan _onErrorDelay;

    public Worker(
        IOptions<AppSettings> settings,
        ILogger<Worker> logger,
        IVkMessageService vkMessageService)
    {
        _setting = settings.Value;
        _logger = logger;

        _onErrorDelay = TimeSpan.FromSeconds(_setting.OnErrorDelay);
        _vkMessageService = vkMessageService;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        // TODO в конце цикла надо очищать
        var needToResponse = await _vkMessageService.GetUnansweredMessagesAsync(token);

        while (true)
        {
            var newMessages = await _vkMessageService.GetNewMessagesAsync(token);
            newMessages = newMessages.Union(needToResponse);

            await ProccessChatsAsync(newMessages, token);
        }
    }

    private async Task ProccessChatsAsync(IEnumerable<Message> messages, CancellationToken token = default)
    {
        var chats = messages.GroupBy(x => x.PeerId);

        if (!chats.Any())
            return;

        foreach (var chat in chats)
        {
            if (chat.Key == null)
                continue;

            var lastMessage = chat.Last();

            if (lastMessage.FromId > 0 && lastMessage.PeerId.HasValue)
            {
                await _vkMessageService.SendMessageAsync(lastMessage.PeerId.Value, lastMessage.Text, true, token);
            }
        }
    }
}
