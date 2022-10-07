using Microsoft.Extensions.Options;
using RateMeBotVk.Configuration;
using System.Threading;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace RateMeBotVk.Services;

public interface IVkLongPollService
{
    Task<LongPollHistoryResponse> GetNewDataAsync(CancellationToken token = default);
}

public class VkLongPollService : IVkLongPollService
{
    private readonly IVkApi _vkApi;
    private readonly AppSettings _settings;

    private MessagesGetLongPollHistoryParams _params;

    public VkLongPollService(IVkApi vkApi, IOptions<AppSettings> settings)
    {
        _vkApi = vkApi;
        _settings = settings.Value;
    }

    public async Task<LongPollHistoryResponse> GetNewDataAsync(CancellationToken token = default)
    {
        if (_params is null)
            await GetLongPollParamsAsync(token);

        var result = await _vkApi.Messages.GetLongPollHistoryAsync(_params);
        _params.Pts = result.NewPts;

        return result;
    }

    private async Task GetLongPollParamsAsync(CancellationToken token = default)
    {
        var server = await _vkApi.Messages.GetLongPollServerAsync(true, _settings.LongPollVersion, _settings.GroupId);
        _params = new MessagesGetLongPollHistoryParams
        {
            Pts = server.Pts,
            GroupId = _settings.GroupId
        };
    }
}
