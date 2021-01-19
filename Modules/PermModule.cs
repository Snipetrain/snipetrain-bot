using System;
using System.Threading.Tasks;
using Discord.Commands;
using snipetrain_bot.Models;
using snipetrain_bot.Services;
using Discord;

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
                var admin = Context.User.ToString();
                var date = DateTimeOffset.Now;
                var kickedUser = user.ToString();
                var userid = user.Id;

                if (user == null)
                {
                    throw new UserNotFound($"Couldn't Find {user}");
                }

                var DBinfo = new PermissionSchema
                {
                    AdminName = admin,
                    Date = date,
                    Reason = reason,
                    User = kickedUser,
                    UserId = userid
                };
                await user.KickAsync(reason);
                await _Permservice.AddKickAsync(DBinfo);
            }
            catch(UserNotFound e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                await ReplyAsync($"Error While Trying to Kick {user}");
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
                var date = DateTimeOffset.Now;
                var kickedUser = user.ToString();
                var userid = user.Id;

                 if (user == null)
                {
                    throw new UserNotFound($"Couldn't Find {user}");
                }

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
            catch(UserNotFound e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                await ReplyAsync($"Error While Trying to Ban {user}");
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
                var date = DateTimeOffset.Now;
                var WarnedUser = user.ToString();
                var userid = user.Id;

                 if (user == null)
                {
                    throw new UserNotFound($"Couldn't Find {user}");
                }

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
            catch(UserNotFound e)
            {
                await ReplyAsync(e.Message);
            }
            catch (Exception e)
            {
                await ReplyAsync($"Error While Trying to Warn {user}");
                Console.WriteLine(e.ToString());
            }
        }
    }
}