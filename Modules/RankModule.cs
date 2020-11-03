using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using snipetrain_bot.Services;

namespace snipetrain_bot.Modules
{
    [Group("rank")]
    public class RankModule : ModuleBase
    {
        private ISnipetrainService _stService;
        private IConfiguration _config;

        public RankModule(IConfiguration configuration, ISnipetrainService stService)
        {
            _config = configuration;
            _stService = stService;
        }

        [Command("player")]
        public async Task GetRankAsync([Remainder] string query) // !rank C A T S
        {
            try
            {
                var res = await _stService.GetRankAsync(query, "tf", 10);

                foreach (var player in res.Payload)
                {
                    decimal KDR = Math.Round((decimal)int.Parse(player.Kills) / int.Parse(player.Deaths), 2);
                    decimal KPM = Math.Round((decimal)int.Parse(player.Kills) / (int.Parse(player.Time) / 60), 2);

                    var embed = new EmbedBuilder
                    {
                        Title = $"{player.Name}",
                        ThumbnailUrl = player.Avatar,
                        Color = Color.DarkRed
                    };

                    embed.AddField("Skill", $"{player.Skill} (Position: #{player.Rank})");
                    embed.AddField("KDR", $"{KDR} ({player.Kills} Kills / {player.Deaths} Deaths)");
                    embed.AddField("KPM", $"{KPM} ({player.Kills} Kills / {int.Parse(player.Time) / 60} Minutes)");
                    embed.AddField("Country", player.Cn);

                    await ReplyAsync("", false, embed.Build());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error while trying to get Rank Data for query <{query}> :: Check Logs");
            }
        }

        [Command("top10")]
        public async Task GetRankTop10Async()
        {
            try
            {
                var res = await _stService.GetTop10Async("tf");
                foreach (var player in res.Payload)
                {
                    decimal KDR = Math.Round((decimal)int.Parse(player.Kills) / int.Parse(player.Deaths), 2);
                    decimal KPM = Math.Round((decimal)int.Parse(player.Kills) / (int.Parse(player.Time) / 60), 2);

                    var embed = new EmbedBuilder
                    {
                        Title = $"{player.Name}",
                        Color = Color.DarkRed
                    };

                    embed.AddField("Skill", $"{player.Skill} (Position: #{player.Rank})");
                    embed.AddField("KDR", $"{KDR} ({player.Kills} Kills / {player.Deaths} Deaths)");

                    await ReplyAsync("", false, embed.Build());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await ReplyAsync($"Error while trying to get Top 10 Rank Data :: Check Logs");
            }
        }
    }
}

