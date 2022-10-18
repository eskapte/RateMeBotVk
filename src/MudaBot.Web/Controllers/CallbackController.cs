using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MudaBot.Core.BotCommandExecuters;
using MudaBot.Web.Configuration;
using MudaBot.Web.Models.Common;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Utils;

namespace MudaBot.Web.Controllers;

[Route("callback")]
[ApiController]
public class CallbackController : ControllerBase
{
    private readonly AppSettings _settings;
    private readonly IMessageCommandExecuter _messageCommandExecuter;
    private readonly ICallbackCommandExecuter _callbackCommandExecuter;

    public CallbackController(
        IOptions<AppSettings> settings,
        IMessageCommandExecuter messageCommandExecuter,
        ICallbackCommandExecuter callbackCommandExecuter)
    {
        _settings = settings.Value;
        _messageCommandExecuter = messageCommandExecuter;
        _callbackCommandExecuter = callbackCommandExecuter;
    }

    [HttpPost]
    public async Task<IActionResult> Callback(UpdateRequest request)
    {
        switch (request.Type)
        {
            case RequestType.Confirmation:
                return IsConfirm(request) ? Ok(_settings.ConfirmString) : BadRequest();
            case RequestType.Callback:
                var msgEvent = MessageEvent.FromJson(new VkResponse(request.Object));
                await _callbackCommandExecuter.ExecuteAsync(msgEvent);
                return Ok();
            case RequestType.NewMessage:
                var message = Message.FromJson(new VkResponse(request.Object));
                await _messageCommandExecuter.ExecuteAsync(message);
                return Ok();
            default:
                return Ok();
        }
    }

    [NonAction]
    private bool IsConfirm(UpdateRequest request) => request.GroupId == _settings.GroupId;
}
