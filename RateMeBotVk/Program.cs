using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace RateMeBotVk
{
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
                    services.AddHostedService<Worker>();
                    services.AddScoped<IVkApi>(x =>
                    {
                        var vk = new VkApi(null);
                        var apiKey = hostContext.Configuration.GetSection("VkApiKey").Value;

                        vk.Authorize(new ApiAuthParams
                        {
                            AccessToken = apiKey
                        });

                        return vk;
                    });
                });
    }
}