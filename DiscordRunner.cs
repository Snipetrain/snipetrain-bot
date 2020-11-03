using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using snipetrain_bot.Modules;
using Microsoft.Extensions.Configuration;
using snipetrain_bot.Services;

namespace snipetrain_bot
{
    class DiscordRunner
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private IConfiguration _config;
        private readonly ITwitchService _twitchService;

        public DiscordRunner(ITwitchService twitchService)
        {
            _commands = new CommandService();
            _twitchService = twitchService;
        }

        public async Task StartClient(IServiceProvider services, IConfiguration config)
        {
            _services = services;
            _config = config;
            _client = new DiscordSocketClient();

            await InstallCommandsAsync();

            await _twitchService.AuthenticateTwitch();

            await _client.LoginAsync(TokenType.Bot, _config["discordToken"]);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommand;
            await _commands.AddModuleAsync<RankModule>(_services);
            await _commands.AddModuleAsync<StreamModule>(_services);
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (!(message.HasCharPrefix(_config["prefixChar"][0], ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var context = new CommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

    }
}
