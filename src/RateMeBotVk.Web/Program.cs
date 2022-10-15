using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using RateMeBotVk.Configuration;
using RateMeBotVk.Core.BotCommandExecuters;
using RateMeBotVk.Services;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<AppSettings>(config.GetSection("AppSettings"));
builder.Services.AddSingleton<IVkApi>(x =>
{
    var vk = new VkApi();
    var apiKey = config.GetSection("VkApiKey").Value;

    vk.Authorize(new ApiAuthParams
    {
        AccessToken = apiKey
    });

    return vk;
});
builder.Services.AddScoped<IMessageCommandExecuter, MessageCommandExecuter>();
builder.Services.AddScoped<IVkSearchInfoService, VkSearchInfoService>();

var app = builder.Build();

//var rwOptions = new RewriteOptions().AddRewrite("/", "callback", false);
//app.UseRewriter(rwOptions);

app.Map("/", () => "Its work");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
