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
using RateMeBotVk.BotCommandExecuter;

namespace RateMeBotVk;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly AppSettings _setting;
    private readonly IVkMessageService _vkMessageService;
    private readonly ICommandExecuter _commandExecuter;

    private readonly TimeSpan _onErrorDelay;

    public Worker(
        IOptions<AppSettings> settings,
        ILogger<Worker> logger,
        IVkMessageService vkMessageService,
        ICommandExecuter commandExecuter)
    {
        _setting = settings.Value;
        _logger = logger;

        _onErrorDelay = TimeSpan.FromSeconds(_setting.OnErrorDelay);
        _vkMessageService = vkMessageService;
        _commandExecuter = commandExecuter;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        var needToResponse = await _vkMessageService.GetUnansweredMessagesAsync(token);
        await ProccessChatsAsync(needToResponse, token);

        while (true)
        {
            var newMessages = await _vkMessageService.GetNewMessagesAsync(token);

            await ProccessChatsAsync(newMessages, token);
        }
    }

    private async Task ProccessChatsAsync(IEnumerable<Message> messages, CancellationToken token = default)
    {
        var lastMessages = messages.GroupBy(x => x.PeerId).Select(x => x.Last());

        if (!lastMessages.Any())
            return;

        foreach (var message in lastMessages)
        {
            await _commandExecuter.ExecuteAsync(message, token);
        }
    }
}
