using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using snipetrain_bot.Services;


namespace snipetrain_bot.Modules
{
    [Group("event")]

    public class StreamEvent : ModuleBase
    {
        [Command("stream")]
        public async Task SendMessag(string message)
        {
            var role = Context.Guild.GetRole(309462947334324224);
            var heartEmoji = new Emoji("\U0001f495");
            var msg =  await ReplyAsync(message);
            var counts = msg.GetReactionUsersAsync(heartEmoji, 100).FlattenAsync();
            System.Console.WriteLine(counts);
        }
        
    }
}