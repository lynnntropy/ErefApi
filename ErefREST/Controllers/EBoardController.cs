using System.Collections.Generic;
using System.Threading.Tasks;
using ErefService;
using ErefService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ErefREST.Controllers
{
    [Produces("application/json")]
    [Route("api/eboard")]
    public class EBoardController : Controller
    {
        private readonly Eref _eref;

        public EBoardController()
        {
            _eref = new Eref();
        }

        [HttpGet("news/{page=1}")]
        public async Task<List<EBoardNewsItem>> News(int page)
        {
            return await _eref.GetNewsAsync(page);
        }
        
        [HttpGet("examples/{page=1}")]
        public async Task<List<EBoardExampleItem>> Examples(int page)
        {
            return await _eref.GetExamplesAsync(page);
        }
        
        [HttpGet("results/{page=1}")]
        public async Task<List<EBoardResultsitem>> Results(int page)
        {
            return await _eref.GetResultsAsync(page);
        }
    }
}