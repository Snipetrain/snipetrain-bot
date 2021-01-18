using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;
using Discord;
using MongoDB.Driver;

namespace snipetrain_bot.Modules
{
    [Group("user")]
    public class PermModule : ModuleBase
    {
        private readonly IPermService _Permservice;
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
                var adminName = Context.User.ToString();
                var date = DateTime.Now;
                var kickedUser = user.ToString();
                var userid = user.Id;

                var DBinfo = new PermissionSchema
                {
                    AdminName = adminName,
                    Date = date,
                    Reason = reason,
                    User = kickedUser,
                    UserId = userid
                };
                await user.KickAsync(reason);
                await _Permservice.AddKickAsync(DBinfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IGuildUser user, [Remainder] string reason)
        {
            try
            {
                var adminName = Context.User.ToString();
                var date = DateTime.Now;
                var kickedUser = user.ToString();
                var userid = user.Id;

                var DBinfo = new PermissionSchema
                {
                    AdminName = adminName,
                    Date = date,
                    Reason = reason,
                    User = kickedUser,
                    UserId = userid
                };

                await user.BanAsync(7, reason, null);
                await _Permservice.AddBanAsync(DBinfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                var date = DateTime.Now;
                var WarnedUser = user.ToString();
                var userid = user.Id;

                var DBinfo = new PermissionSchema
                {
                    AdminName = adminName,
                    Date = date,
                    Reason = reason,
                    User = WarnedUser,
                    UserId = userid
                };
                await _Permservice.AddWarnAsync(DBinfo);

                var docNum = await _Permservice.GetDocsAsync(user);
                if (docNum == 3)
                {
                    await user.KickAsync(reason, null);

                }
                if (docNum == 5)
                {
                    await user.BanAsync(7, reason, null);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}