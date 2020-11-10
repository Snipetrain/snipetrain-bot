using System.Collections.Generic;
using System.Threading.Tasks;
using snipetrain_bot.Models;
using Discord;

namespace snipetrain_bot.Services
{
    public interface IPermService
    {
        Task AddKickAsync(PermissionSchema model);
        Task AddBanAsync(PermissionSchema model);
        Task AddWarnAsync(PermissionSchema model);
        Task<List<PermissionSchema>> GetDocsAsync();
        Task<PermissionSchema> GetDocsAsync(IGuildUser user);

    }
}