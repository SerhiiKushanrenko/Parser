using Microsoft.AspNetCore.Mvc;
using Parser1.EF;
using Parser1.Interfaces;

namespace Parser1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ISupportParser _parser;

        public WorkController(ApplicationContext context, ISupportParser parser)
        {
            _context = context;
            _parser = parser;
        }

        /// <summary>
        /// General Parser Work From Site
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {

            _parser.AddWorkToScientists("педагогічні науки");
            return Ok();
        }

        /// <summary>
        /// Search scientist to work
        /// </summary>
        /// <param name="generalWork"></param>
        /// <returns></returns>
        [HttpGet("generalWork")]
        public IActionResult GetGeneralWork(string generalWork)
        {
            try
            {
                var workOfScientistId = _context.WorkOfScientists.FirstOrDefault(e => e.Name.Equals(generalWork))!.Id;
                var scientists = _context.ScientistsWork.Where(e => e.WorkOfScientistId == workOfScientistId).Select(q => q.Scientist).ToList();
                return Ok(scientists);
            }
            catch (System.NullReferenceException e)
            {
                var a = $"{e.Message} такой работы нет ";
                return Ok(a);
            }
        }
    }
}
