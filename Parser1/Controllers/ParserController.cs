using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Parser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParserController : ControllerBase
    {
        private readonly IMainParser _mainParser;
        private readonly ISupportParser _supportParser;
        private readonly IRatingService ratingService;

        public ParserController(IMainParser mainParser, ISupportParser supportParser, IRatingService ratingService)
        {
            _mainParser = mainParser;
            _supportParser = supportParser;
            this.ratingService = ratingService;
        }


        [HttpGet("Parser")]
        public async Task<IActionResult> StartMainParser()
        {
            await _mainParser.StartParsing();
            return Ok();
        }

        /// <summary>
        /// Get and Check all Direction
        /// </summary>
        /// <returns></returns>
        //[HttpGet("GetDirection")]
        //public IActionResult GetAllDirection()
        //{
        //    var directions = _mainParser.GetDirection();

        //    return Ok(directions);
        //}

        /// <summary>
        /// Search scientist to work
        /// </summary>
        /// <param name="generalWork"></param>
        /// <returns></returns>
        [HttpGet("ParserWork")]
        public IActionResult ParserOnlyWork(string direction)
        {
            _supportParser.AddWorkToScientists(direction);
            return Ok();
        }
    }
}
