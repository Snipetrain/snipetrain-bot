using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using snipetrain_bot.Models;
using snipetrain_bot.Services;

namespace snipetrain_bot.Modules
{
    [Group("stream")]

    public class StreamModule : ModuleBase
    {
        private readonly IStreamersService _streamersService;
        private readonly ITwitchService _twitchService;

        public StreamModule(IStreamersService streamersService, ITwitchService twitchService)
        {
            _streamersService = streamersService;
            _twitchService = twitchService;
        }

        [Command("add")]
        public async Task AddStream([Remainder] string name)
        {
            try
            {

                var dbStreamer = await _streamersService.GetStreamerAsync(name);

                if (dbStreamer != null)
                    throw new StreamerAlreadyExistsException("Streamer already exists in DB!");

                var twitchUser = await _twitchService.GetTwitchUser(name);

                _twitchService.AddTwitchSubscription(name, twitchUser.Id);

                var streamer = new StreamersSchema()
                {
                    Name = name,
                    TwitchUserId = twitchUser.Id
                };

                await _streamersService.AddStreamerAsync(streamer);

                await ReplyAsync($"Succesfully added streamer <{name}> !");
            }
            catch (StreamerAlreadyExistsException e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error while trying to add a streamer :: Check Logs");
            }
        } 

        [Command("remove")]
        public async Task DeleteStream([Remainder] string name)
        {
            try
            {
                var streamer = await _streamersService.GetStreamerAsync(name);

                if (streamer == null)
                    throw new StreamerDoesntExistsException("Streamer doesn't exists in DB!");

                _twitchService.RemoveTwitchSubscription(name);

                await _streamersService.DeleteStreamerAsync(name);

                await ReplyAsync($"Succesfully Removed streamer <{name}> !");
            }
            catch (StreamerDoesntExistsException e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error while trying to add a streamer :: Check Logs");
            }
        }

    }
}