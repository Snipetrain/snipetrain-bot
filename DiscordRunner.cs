﻿using System;
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
    public class DiscordRunner
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private readonly IPartyService _partyService;
        private IConfiguration _config;
        private readonly ITwitchService _twitchService;
        private SocketGuild Guild;

        public DiscordRunner(ITwitchService twitchService, IConfiguration config, IPartyService partyService)
        {
            _commands = new CommandService();
            _twitchService = twitchService;
            _config = config;
            _partyService = partyService;

        }

        public async Task StartClient(IServiceProvider services)
        {
            _services = services;

            _client = new DiscordSocketClient();

            await InstallCommandsAsync();

            await _twitchService.AuthenticateTwitch();
            await _twitchService.AddAllTwitchSubscriptions();

            await _client.LoginAsync(TokenType.Bot, _config.GetSection("discord")["token"]);
            await _client.StartAsync();

            await _client.DownloadUsersAsync(_client.Guilds);
            // Guild = _client.GetGuild(ulong.Parse(_config.GetSection("discord")["guildId"]));


            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommand;
            _client.ReactionAdded += OnReactionUp;
            _client.ReactionAdded += ReactionMonitoring;
            _client.ReactionRemoved += OnReactionDown;
            _client.GuildAvailable += OnGuildAvailable;

            await _commands.AddModuleAsync<RankModule>(_services);
            await _commands.AddModuleAsync<StreamModule>(_services);
            await _commands.AddModuleAsync<PartyModule>(_services);
            await _commands.AddModuleAsync<PermModule>(_services);

        }

        public async Task OnGuildAvailable(SocketGuild guild)
        {
            Guild = guild;
        }

        public async Task<IMessage> SendMessage(string message, ulong channelId)
        {
            try
            {
                var socketChannel = _client.GetChannel(channelId) as IMessageChannel;
                return await socketChannel.SendMessageAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to send Message to Channel :: {e.ToString()}");
                return null;
            }
        }

        public async Task<IMessage> SendDMMessage(string message, IUser user)
        {
            try
            {
                return await user.SendMessageAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to send Message to Channel :: {e.ToString()}");
                return null;
            }
        }

        public async Task OnReactionUp(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {

                var messageId = ulong.Parse(_config.GetSection("discord").GetSection("messages")["streamReaction"]);
                var reactionCode = _config.GetSection("discord").GetSection("emotes")["stream"];

                if (message.Id == messageId && reaction.Emote.Name == reactionCode)
                {
                    var role = Guild.GetRole(ulong.Parse(_config.GetSection("discord").GetSection("roles").GetSection("stream")["id"]));
                    var guildUser = Guild.GetUser(reaction.UserId);

                    await (guildUser as IGuildUser).AddRoleAsync(role);
                    await SendDMMessage($"Successfully Added you to the <{role.Name}> Role.", guildUser);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }


        public async Task OnReactionDown(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            try
            {
                if (message.Id == ulong.Parse(_config.GetSection("discord").GetSection("messages")["streamReaction"]))
                {
                    var role = Guild.GetRole(ulong.Parse(_config.GetSection("discord").GetSection("roles").GetSection("stream")["id"]));
                    var guildUser = Guild.GetUser(reaction.UserId);

                    await (guildUser as IGuildUser).RemoveRoleAsync(role);
                    await SendDMMessage($"Successfully Removed you from the <{role.Name}> Role.", guildUser);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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

        public async Task ReactionMonitoring(Cacheable<IUserMessage, ulong> message,ISocketMessageChannel channel, SocketReaction reaction)
        {
            var votingParty = await _partyService.GetVotingPartyAsync();
            var votingPartyId = votingParty.MessageId;
            var reactionCode = _config.GetSection("discord").GetSection("emotes")["vote"];


            if (message.Id == votingPartyId && reaction.Emote.Name == reactionCode && channel.Name == "eu-channel" || channel.Name == "na-channel")
            {
                var partyMessage = await channel.GetMessageAsync(votingPartyId);
                if (partyMessage.Reactions.Count >= 2)
                {
                    await _partyService.UpdatePartyStateAsync(votingParty, Models.PartyState.VOTING);
                    await SendMessage("IT WORKED",765288858601521172);
                }
            }
        }

    }
}
