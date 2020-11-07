using System.Collections.Generic;
using System.Threading.Tasks;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public interface ISnipetrainService
    {
        Task<Pagination<List<Player>>> GetRankAsync(string query, string game, int perPage);
        Task<Pagination<List<Player>>> GetTop10Async(string game);
    }
}