using System.Collections.Generic;
using System.Threading.Tasks;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public interface IStreamersService
    {
        Task<List<StreamersSchema>> GetStreamersAsync();
        Task<StreamersSchema> GetStreamerAsync(string name);
        Task AddStreamerAsync(StreamersSchema model);
        Task DeleteStreamerAsync(string name);
    }
}