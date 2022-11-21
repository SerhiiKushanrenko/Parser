using BLL.Parsers.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace Parser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParserController : ControllerBase
    {
        private readonly IMainParser _mainParser;
        private readonly INbuviapParser _inbuviapParser;

        public ParserController(IMainParser mainParser, INbuviapParser inbuviapParser)
        {
            _mainParser = mainParser;

            _inbuviapParser = inbuviapParser;
        }


        [HttpGet("StartParser")]
        public async Task<IActionResult> StartMainParser()
        {
            await _inbuviapParser.StartParsing();
            return Ok("Parsing succesfully started");
        }

        [HttpGet("StartParserWithParametrs")]
        public async Task<IActionResult> StartMainParserWithP(ParsingType type, string? scientistSecondName)
        {
            await _mainParser.StartParsing(type, scientistSecondName);
            return Ok("Parsing succesfully started");
        }

        /// <summary>
        /// Get and Check all Direction
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDirection")]
        public IActionResult GetAllDirection()
        {
            var directions = _inbuviapParser.GetDirection();

            return Ok(directions);
        }

    }
}
