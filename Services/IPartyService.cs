
using System.Collections.Generic;
using System.Threading.Tasks;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public interface IPartyService 
    {
        Task<List<PartySchema>> GetPartiesAsync();
        Task<PartySchema> GetPartyAsync(string id);
        Task AddPartyAsync(PartySchema model);
        Task RemovePartyAsync(string id);
    }
   
}