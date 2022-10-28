using BLL.Interfaces;
using BLL.Parsers.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace Parser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScientistController : ControllerBase
    {

        private readonly IMainParser _mainParser;
        private readonly ISupportParser _supportParser;
        public ScientistController(IMainParser mainParser, ISupportParser supportParser)
        {

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
            //var user = await _context.Scientists.FindAsync(id);

            //if (user is null)
            //{
            //    return Ok($"Юзера с ID{id} нет в БД");

            //    //_mainParser.ParseGeneralInfo();
            //    //Thread.Sleep(2000);
            //    //return Ok();
            //}
            return Ok();
        }

        /// <summary>
        /// Get all Scientist from DB 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scientist>>> GetAll()
        {
            //var scientists = await _context.Scientists.Take(500).AsNoTracking().ToListAsync();
            //return Ok(scientists);
            return Ok();
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
            //var result = _context.Scientists.Skip(haveToSkip).Take(haveToGet).ToList();
            //return result.Any() ? Ok(result) : Ok("Стільки вчених немає");
            return Ok();
        }

        [HttpGet("GetScientistFromWork")]
        public IActionResult GetScientistFromWork(string generalWork)
        {
            return Ok();
            //try
            //{
            //    var workOfScientistId = _context.Works.FirstOrDefault(e => e.Name.Equals(generalWork))!.Id;
            //    var scientists = _context.ScientistsWork.Where(e => e.WorkId == workOfScientistId).Select(q => q.Scientist).ToList();
            //    return Ok(scientists);
            //}
            //catch (System.NullReferenceException e)
            //{
            //    var a = $"{e.Message} такой работы нет ";
            //    return Ok(a);
            //}
        }

        //[HttpGet("GetAllFromDirection")]
        //public async Task<IActionResult> GetAllFromDirection(string direction)
        //{
        //    // проверка на обновление бд на сайте
        //    // mainParser.CheckOnEquals(direction);
        //    try
        //    {
        //        var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;
        //        var scientists = await _context.Scientists.Where(e => e.DirectionId == directionId).Take(30).ToListAsync();
        //        return Ok(scientists);
        //    }
        //    catch (Exception e)
        //    {
        //        var a = $"{e.Message} такого направления нет ";
        //        return Ok(a);
        //    }
        //}

        /// <summary>
        /// Get Scientists for Organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpGet("GetAllFromOrganization")]
        public IActionResult GetAllFromOrganization(string organization)
        {
            try
            {
                // List<Scientist> scientists = _context.Scientists.Where(e => e.Organization == organization).ToList();
                return Ok();
            }
            catch (Exception e)
            {
                var a = $"{e.Message} такого направления нет ";
                return Ok(a);
            }
        }
    }
}