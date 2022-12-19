using BLL.AdditionalModels;
using BLL.Services.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;


namespace Parser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScientistController : ControllerBase
    {
        private readonly IScientistService _scientistService;

        public ScientistController(IScientistService scientistService)
        {
            _scientistService = scientistService;
        }

        /// <summary>
        /// Get specific scientist using Scientist.Id to search
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _scientistService.GetAsync(id);

            return user is null ? BadRequest($"User with Id = {id} not found") : Ok(user);
        }

        /// <summary>
        /// Get scientists list using filtering options in the filter object
        /// </summary>
        /// <param name="filter">Used to provide filtering options</param>
        /// <returns>List of scientists that were found</returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Scientist>>> GetScientistsAsync(ScientistFilter filter)
        {
            return Ok(await _scientistService.GetScientistsAsync(filter));
        }
    }
}