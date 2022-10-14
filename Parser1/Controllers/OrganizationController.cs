using Microsoft.AspNetCore.Mvc;
using Parser1.EF;
using Parser1.Interfaces;
using Parser1.Models;

namespace Parser1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMainParser _mainParser;

        public OrganizationController(ApplicationContext context, IMainParser mainParser)
        {
            _context = context;
            _mainParser = mainParser;
        }

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
                List<Scientist> scientists = _context.Scientists.Where(e => e.Organization == organization).ToList();
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
