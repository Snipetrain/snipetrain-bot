using System.Collections.Generic;
using System.Threading.Tasks;
using snipetrain_bot.Models;

namespace snipetrain_bot.Services
{
    public interface IEventService 
    {
        Task<List<EventSchema>> GetDocAsync();
        Task<EventSchema> GetDocAsync(string id);
        Task DeleteDocAsync(string id);
        Task AddEventAsync(EventSchema model);
    }
   
}