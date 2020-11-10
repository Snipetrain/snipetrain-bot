using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;
using Discord;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace snipetrain_bot.Modules
{
    [Group("user")]
    public class PermModule : ModuleBase
    {
        private readonly IPermService _Permservice;
        private readonly IMongoCollection<PermissionSchema> _Warndoc;
        public PermModule(IPermService permService)
        {
            _Permservice = permService;
        }
        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(IGuildUser user, [Remainder] string reason)
        {
            try
            {
                await user.KickAsync(reason);
                var adminName = Context.User.ToString();
                var date = DateTime.Now.ToString();
                var kickedUser = user.ToString();
                var DBinfo = new PermissionSchema
                {
                    AdminName = adminName,
                    Date = date,
                    Reason = reason,
                    User = kickedUser
                };
                await _Permservice.AddKickAsync(DBinfo);
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.ToString());
            }
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IGuildUser user, [Remainder] string reason)
        {
            try
            {
                await user.BanAsync(7, reason, null);
                var adminName = Context.User.ToString();
                var date = DateTime.Now.ToString();
                var kickedUser = user.ToString();
                var DBinfo = new PermissionSchema
                {
                    AdminName = adminName,
                    Date = date,
                    Reason = reason,
                    User = kickedUser
                };
                await _Permservice.AddBanAsync(DBinfo);
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.ToString());
            }
        }
        [Command("warn")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task WarnAsync(IGuildUser user, [Remainder] string reason)
        {
            try
            {
                var adminName = Context.User.ToString();
                var date = DateTime.Now.ToString();
                var WarnedUser = user.ToString();
                var DBinfo = new PermissionSchema
                {
                    AdminName = adminName,
                    Date = date,
                    Reason = reason,
                    User = WarnedUser
                };
                await _Permservice.AddWarnAsync(DBinfo);
                var docNum = await _Permservice.GetDocsAsync(user);

            }
            catch
            {
            }
        }
    }
}