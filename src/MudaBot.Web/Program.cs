using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudaBot.Core.BotCommandExecuters;
using MudaBot.Core.Services;
using MudaBot.DataAccess;
using MudaBot.Web.Configuration;
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
builder.Services.AddScoped<ICallbackCommandExecuter, CallbackCommandExecuter>();
builder.Services.AddScoped<IVkSearchInfoService, VkSearchInfoService>();
builder.Services.AddScoped<IUserInteractionService, UserInteractionService>();

builder.Services.AddDbContext<Db>(options =>
{
    options.UseSqlServer(config.GetConnectionString("VkBotDatabase"), config => config.CommandTimeout(60));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
