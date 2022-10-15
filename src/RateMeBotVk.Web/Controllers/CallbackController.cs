using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RateMeBotVk.Configuration;
using RateMeBotVk.Core.BotCommandExecuters;
using RateMeBotVk.Web.Models.Common;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Utils;

namespace RateMeBotVkWeb.Controllers;

[Route("callback")]
[ApiController]
public class CallbackController : ControllerBase
{
    private readonly AppSettings _settings;
    private readonly IMessageCommandExecuter _messageCommandExecuter;

    public CallbackController(IOptions<AppSettings> settings, IMessageCommandExecuter messageCommandExecuter)
    {
        _settings = settings.Value;
        _messageCommandExecuter = messageCommandExecuter;
    }

    [HttpPost]
    public async Task<IActionResult> Callback(UpdateRequest request)
    {
        switch (request.Type) {
            case RequestType.Confirmation:
                return IsConfirm(request) ? Ok(_settings.ConfirmString) : BadRequest();
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
