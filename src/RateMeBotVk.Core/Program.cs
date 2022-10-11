﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RateMeBotVk.BotCommandExecuter;
using RateMeBotVk.Configuration;
using RateMeBotVk.DataAccess;
using RateMeBotVk.Services;
using System.Threading.Tasks;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace RateMeBotVk;

internal class Program
{
    static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IVkLongPollService, VkLongPollService>();
                services.AddSingleton<IVkMessageService, VkMessageService>();
                services.AddSingleton<IVkApi>(x =>
                {
                    var vk = new VkApi(services);
                    var apiKey = hostContext.Configuration.GetSection("VkApiKey").Value;

                    vk.Authorize(new ApiAuthParams
                    {
                        AccessToken = apiKey
                    });

                    return vk;
                });
                services.AddScoped<IVkSearchInfoService, VkSearchInfoService>();
                services.AddTransient<ICommandExecuter, CommandExecuter>();
                services.AddHostedService<Worker>();
                services.Configure<AppSettings>(hostContext.Configuration.GetSection(nameof(AppSettings)));
                services.AddDbContext<Db>(options =>
                {
                    options.UseSqlServer(hostContext.Configuration.GetConnectionString("VkBotDatabase"), builder =>
                    {
                        builder.CommandTimeout(60);
                    });
                });
            });
}