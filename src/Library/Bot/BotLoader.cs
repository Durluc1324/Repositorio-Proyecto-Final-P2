using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClassLibrary
{
    public static class BotLoader
    {
        public static async Task LoadAsync()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();


            var services = new ServiceCollection()
                .AddLogging(options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                })
                .AddSingleton<IConfiguration>(configuration)

                // 🔹 Estas son las líneas que te faltaban
                .AddSingleton<SessionService>()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()

                .AddSingleton<IBot, Bot>();

            // Construimos el ServiceProvider
            var serviceProvider = services.BuildServiceProvider();

            try
            {
                IBot bot = serviceProvider.GetRequiredService<IBot>();

                await bot.StartAsync(serviceProvider);

                Console.WriteLine("Conectado a Discord. Presione 'q' para salir...");

                do
                {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key != ConsoleKey.Q) continue;

                    Console.WriteLine("\nFinalizado");
                    await bot.StopAsync();

                    return;
                } while (true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit(-1);
            }
        }
    }
}