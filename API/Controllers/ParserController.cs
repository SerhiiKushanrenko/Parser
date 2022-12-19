using BLL.AdditionalModels;
using BLL.Parsers.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Parser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParserController : ControllerBase
    {
        private readonly IParsingHandler _parsingHandler;

        public ParserController(IParsingHandler parsingHandler)
        {
            _parsingHandler = parsingHandler;
        }


        [HttpGet("StartParser")]
        public async Task<IActionResult> StartParser(ParsingType type)
        {
            await _parsingHandler.StartParsing(type);
            return Ok("Parsing succesfully started");
        }
    }
}
