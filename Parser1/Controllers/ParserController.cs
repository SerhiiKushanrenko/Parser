using Microsoft.AspNetCore.Mvc;
using Parser1.EF;
using Parser1.Interfaces;

namespace Parser1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMainParser _mainParser;
        private readonly ISupportParser _supportParser;
        private readonly IRatingServise _ratingServise;

        public ParserController(ApplicationContext context, IMainParser mainParser, ISupportParser supportParser, IRatingServise ratingServise)
        {
            _context = context;
            _mainParser = mainParser;
            _supportParser = supportParser;
            _ratingServise = ratingServise;
        }


        [HttpGet("Start Main Parser")]
        public async Task<IActionResult> StartMainParser()
        {
            _mainParser.ParseGeneralInfo();
            return Ok();
        }

        /// <summary>
        /// Get and Check all Direction
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get and Check all Direction")]
        public IActionResult GetAllDirection()
        {
            var directions = _mainParser.GetDirection();

            return Ok(directions);
        }

        /// <summary>
        /// Search scientist to work
        /// </summary>
        /// <param name="generalWork"></param>
        /// <returns></returns>
        [HttpGet("ParserOnlyWork")]
        public IActionResult ParserOnlyWork(string direction)
        {

            _supportParser.AddWorkToScientists(direction);
            return Ok();
        }
    }
}
