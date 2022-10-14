using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parser1.EF;
using Parser1.Interfaces;

namespace Parser1.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class DirectionController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMainParser _mainParser;

        public DirectionController(ApplicationContext context, IMainParser mainParser)
        {
            _context = context;
            _mainParser = mainParser;
        }
        /// <summary>
        /// Get and Check all Direction
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var directions = _mainParser.GetDirection();

            return Ok(directions);
        }

        /// <summary>
        /// Get Scientist from direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllFromDirection(string direction)
        {
            // проверка на обновление бд на сайте
            // mainParser.CheckOnEquals(direction);
            try
            {
                var directionId = _context.Directions.FirstOrDefault(e => e.Name.Equals(direction))!.Id;
                var scientists = await _context.Scientists.Where(e => e.DirectionId == directionId).Take(30).ToListAsync();
                return Ok(scientists);
            }
            catch (Exception e)
            {
                var a = $"{e.Message} такого направления нет ";
                return Ok(a);
            }
        }
    }
}
