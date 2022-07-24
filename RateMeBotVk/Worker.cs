using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VkNet.Abstractions;
using VkNet.Model;
using Microsoft.Extensions.Logging;
using VkNet.Model.RequestParams;
using VkNet.Enums;
using System.Diagnostics;

namespace RateMeBotVk
{
    public class Worker : BackgroundService
    {
        private readonly IVkApi _vkApi;
        private readonly ILogger<Worker> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IConfiguration _config;

        private readonly ulong _groupId;
        private readonly uint _apiVersion;
        private readonly TimeSpan _onErrorDelay;

        public Worker(IVkApi vkApi, IHostApplicationLifetime appLifetime, IConfiguration config, ILogger<Worker> logger)
        {
            _config = config;
            _vkApi = vkApi;
            _appLifetime = appLifetime;
            _logger = logger;
            _groupId = ulong.Parse(_config["GroupId"]);
            _apiVersion = uint.Parse(_config["LongPoolVersion"]);
            _onErrorDelay = TimeSpan.FromSeconds(int.Parse(_config["OnErrorDelaySeconds"]));
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            var server = await _vkApi.Messages.GetLongPollServerAsync(true, _apiVersion, _groupId);
            var pollParams = GetMessagesLongPollParams(server);

            var conversations = await _vkApi.Messages.GetConversationsAsync(new());

            if (conversations != null)
            {
                await ProccessUnanswered(conversations.Items);
                await ProccessUnreaded(conversations.Items);
            }

            LongPollHistoryResponse? poll = null;
            while (true)
            {
                try
                {
                    poll = await _vkApi.Messages.GetLongPollHistoryAsync(pollParams);
                    pollParams = GetMessagesLongPollParams(poll.NewPts);

                    if (poll.UnreadMessages == 0)
                    {
                        continue;
                    }

                    var chats = poll.Messages.GroupBy(x => x.PeerId);

                    if (chats.Count() == 0)
                        continue;

                    foreach (var chat in chats)
                    {
                        if (chat.Key == null)
                        {
                            continue;
                        }

                        var lastMessage = chat.Last();
                        
                        if (lastMessage.FromId > 0 && lastMessage.PeerId.HasValue)
                        {
                            await SendMessageAsync(lastMessage.PeerId.Value, lastMessage.Text);
                        }
                    }
                }
                catch (Exception err)
                {
                    _logger.LogError(err.Message);
                    await Task.Delay(_onErrorDelay, token);
                }
            }
        }

        private async Task ProccessUnanswered(IEnumerable<ConversationAndLastMessage> chats)
        {
            var unanswered = chats.Where(x => x.Conversation.Unanswered
                                        && (!x.Conversation.UnreadCount.HasValue || x.Conversation.UnreadCount == 0));

            foreach(var chat in unanswered)
            {
                var lastMessage = chat.LastMessage.Text;
                var peer = chat.Conversation.Peer;

                await SendMessageAsync(peer.Id, "Простите за ожидание. Я был немного не в состоянии ответить", false);
                await SendMessageAsync(peer.Id, lastMessage, false);
            }
        }

        private async Task ProccessUnreaded(IEnumerable<ConversationAndLastMessage> chats)
        {
            var unreaded = chats.Where(x => x.Conversation.UnreadCount > 0);

            foreach (var chat in unreaded)
            {
                var lastMessage = chat.LastMessage.Text;
                var peer = chat.Conversation.Peer;

                await SendMessageAsync(peer.Id, lastMessage);
            }
        }

        private async Task<long> SendMessageAsync(long peerId, string message, bool needToRead = true)
        {
            if (needToRead)
            {
                var isReaded = await _vkApi.Messages.MarkAsReadAsync(peerId.ToString());
                if (!isReaded)
                    throw new Exception($"Не удалось прочитать сообщение пользователя {peerId}.");
            }

            var msgParams = new MessagesSendParams()
            {
                PeerId = peerId,
                RandomId = GetRandomId(),
                Message = message
            };

            return await _vkApi.Messages.SendAsync(msgParams);
        }

        private MessagesGetLongPollHistoryParams GetMessagesLongPollParams(LongPollServerResponse server) =>
            new MessagesGetLongPollHistoryParams()
            {
                Pts = server?.Pts,
                GroupId = _groupId
            };

        private MessagesGetLongPollHistoryParams GetMessagesLongPollParams(ulong pts) =>
            new MessagesGetLongPollHistoryParams()
            {
                Pts = pts,
                GroupId = _groupId
            };

        private int GetRandomId() => DateTime.Now.GetHashCode();
    }
}
