using System.Collections.Generic;
using System.Threading.Tasks;
using ErefService;
using ErefService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ErefREST.Controllers
{
    [Produces("application/json")]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        private readonly Eref _eref;

        public ScheduleController()
        {
            _eref = new Eref();
        }

        [HttpGet]
        public async Task<List<ScheduleListItem>> List()
        {
            return await _eref.GetScheduleListAsync();
        }

        [HttpGet("{groupId}")]
        public async Task<List<ScheduleItem>> Schedule(int groupId)
        {
            return await _eref.GetScheduleForGroupAsync(groupId);            
        }
    }
}