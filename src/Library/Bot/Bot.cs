using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
namespace ClassLibrary;

public class Bot : IBot
    {
        private ServiceProvider serviceProvider;
        private readonly ILogger<Bot> logger;
        private readonly IConfiguration configuration;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        public Bot(ILogger<Bot> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;

            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = true,
                GatewayIntents =
                    GatewayIntents.AllUnprivileged
                    | GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(config);
            commands = new CommandService();
        }

        public async Task StartAsync(ServiceProvider services)
        {
            string discordToken = configuration["DiscordToken"];
            if (discordToken == null)
            {
                throw new Exception("Falta el token");
            }

            logger.LogInformation($"Iniciando el con token {discordToken}");

            serviceProvider = services;

            await commands.AddModulesAsync(Assembly.GetExecutingAssembly(),
                serviceProvider);

            await client.LoginAsync(TokenType.Bot, discordToken);
            await client.StartAsync();

            client.MessageReceived += HandleCommandAsync;
        }

        public async Task StopAsync()
        {
            logger.LogInformation("Finalizando");
            await client.LogoutAsync();
            await client.StopAsync();
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot)
            {
                return;
            }

            int position = 0;
            bool messageIsCommand = message.HasCharPrefix('!', ref position);

            if (messageIsCommand)
            {
                await commands.ExecuteAsync(
                    new SocketCommandContext(client, message),
                    position,
                    serviceProvider);
            }
        }
    }