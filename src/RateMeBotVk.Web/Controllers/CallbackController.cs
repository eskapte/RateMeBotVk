using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RateMeBotVkWeb.Controllers;

[Route("[controller]")]
[ApiController]
public class CallbackController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Callback()
    {
        return Ok();
    }
}
