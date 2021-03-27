
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public interface IPartyService 
    {
        Task<List<PartySchema>> GetPartiesAsync();
        Task<List<PartySchema>> GetDailyPartiesAsync();
        Task<PartySchema> GetPartyAsync(string id);
        Task AddPartyAsync(PartySchema model);
        Task RemovePartyAsync(string id);
        Task UpdatePartyStateAsync(PartySchema party,PartyState PartyState);
        Task<PartySchema> GetVotingPartyAsync();
    }
   
}