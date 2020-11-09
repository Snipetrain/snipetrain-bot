using System.Collections.Generic;
using System.Threading.Tasks;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public interface IKickService
    {
        Task AddKickAsync(KickSchema model);
    }
}