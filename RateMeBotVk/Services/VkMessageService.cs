using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Enums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Services;

public interface IVkMessageService
{
    public Task<IEnumerable<Message>> GetUnansweredMessagesAsync(CancellationToken token = default);
    public Task<IEnumerable<Message>> GetNewMessagesAsync(CancellationToken token = default);
    public Task<long> SendMessageAsync(long peerId, string message, bool needToRead = true, CancellationToken token = default);
}

public class VkMessageService : IVkMessageService
{
    private readonly IVkApi _vkApi;
    private readonly IVkLongPollService _vkLongPollService;

    public VkMessageService(IVkApi vkApi, IVkLongPollService vkLongPollService)
    {
        _vkApi = vkApi;
        _vkLongPollService = vkLongPollService;
    }

    public async Task<IEnumerable<Message>> GetNewMessagesAsync(CancellationToken token = default)
    {
        var apiResponse = await _vkLongPollService.GetNewDataAsync(token);
        var newMessages = apiResponse.Messages.Where(x => x.Type == MessageType.Received);

        return newMessages;
    }

    public async Task<IEnumerable<Message>> GetUnansweredMessagesAsync(CancellationToken token = default)
    {
        var conversations = await _vkApi.Messages.GetConversationsAsync(new());

        var unanswered = conversations.Items
            .Where(x => x.Conversation.Unanswered && x.Conversation.UnreadCount == 0)
            .Select(x => x.LastMessage);
        var unreaded = conversations.Items
            .Where(x => x.Conversation.UnreadCount > 0)
            .Select(x => x.LastMessage);

        return unanswered.Union(unreaded);
    }

    public async Task<long> SendMessageAsync(
        long peerId, 
        string message, 
        bool needToRead = true, 
        CancellationToken token = default)
    {
        if (needToRead)
        {
            var isReaded = await _vkApi.Messages.MarkAsReadAsync(peerId.ToString());
            if (!isReaded)
                throw new Exception($"Failed to read message from peerId {peerId}.");
        }

        if (string.IsNullOrEmpty(message))
            return 0;

        var msgParams = new MessagesSendParams
        {
            PeerId = peerId,
            RandomId = GetRandomId(),
            Message = message
        };

        return await _vkApi.Messages.SendAsync(msgParams);
    }

    private int GetRandomId() => DateTime.Now.GetHashCode();
}
