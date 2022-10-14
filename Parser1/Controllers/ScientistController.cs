using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser1.EF;
using Parser1.Interfaces;
using Parser1.Models;

namespace Parser1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScientistController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMainParser _mainParser;
        private readonly ISupportParser _supportParser;
        public ScientistController(ApplicationContext context, IMainParser mainParser, ISupportParser supportParser)
        {
            _context = context;
            _mainParser = mainParser;
            _supportParser = supportParser;
        }

        /// <summary>
        /// Parser scientists for random direction
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Scientists.FindAsync(id);

            if (user is null)
            {
                _supportParser.GetGeneralInfo("педагогічні науки", "Педагогіка");
                Thread.Sleep(2000);
                return Ok();
            }
            return Ok(user);
        }

        /// <summary>
        /// Get all Scientist from DB 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scientist>>> GetAll()
        {
            var scientists = await _context.Scientists.Take(500).AsNoTracking().ToListAsync();
            return Ok(scientists);
        }

        /// <summary>
        /// Get Scientists From To Order 
        /// </summary>
        /// <param name="haveToGet"></param>
        /// <param name="haveToSkip"></param>
        /// <returns></returns>
        [HttpGet("GetFromOrder")]
        public IActionResult GetFromOrder(int haveToGet, int haveToSkip)
        {
            var result = _context.Scientists.Skip(haveToSkip).Take(haveToGet).ToList();
            return result.Any() ? Ok(result) : Ok("Стільки вчених немає");
        }
    }
}