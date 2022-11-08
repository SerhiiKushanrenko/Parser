using BLL.Interfaces;
using BLL.Parsers.Interfaces;
using DAL.AdditionalModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Parser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScientistController : ControllerBase
    {
        private readonly IWorkRepository _workRepository;
        private readonly IScientistWorkRepository _scientistWorkRepository;
        private readonly IMainParser _mainParser;
        private readonly ISupportParser _supportParser;
        private readonly IScientistRepository _scientistRepository;
        private readonly IFieldOfResearchRepository _fieldOfResearchRepository;
        private readonly IScientistFieldOfResearchRepository _scientistFieldOfResearchRepository;

        public ScientistController(
            IMainParser mainParser,
            ISupportParser supportParser,
            IScientistRepository scientistRepository,
            IFieldOfResearchRepository fieldOfResearchRepository,
            IScientistFieldOfResearchRepository scientistFieldOfResearchRepository,
            IScientistWorkRepository scientistWorkRepository,
            IWorkRepository workRepository
        )
        {

            _mainParser = mainParser;
            _supportParser = supportParser;
            _scientistRepository = scientistRepository;
            _fieldOfResearchRepository = fieldOfResearchRepository;
            _scientistFieldOfResearchRepository = scientistFieldOfResearchRepository;
            _scientistWorkRepository = scientistWorkRepository;
            _workRepository = workRepository;

        }

        /// <summary>
        /// Parser scientists for random fieldOfResearch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _scientistRepository.GetAsync(id);

            return user is null ? Ok($"Юзера с ID{id} нет в БД") : Ok(user);
        }

        /// <summary>
        /// Get all Scientist from DB 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scientist>>> GetAll()
        {
            var scientists = await _scientistRepository.GetAll().AsNoTracking().ToListAsync();
            return Ok(scientists);

        }


        [HttpGet("GetAllFromDirection")]
        public async Task<IActionResult> GetAllFromDirection(string fieldOfResearch)
        {

            try
            {
                var currentDirection = await _fieldOfResearchRepository.GetAsync(fieldOfResearch);
                var result = await _scientistFieldOfResearchRepository.GetScientistsFieldsOfResearchAsync(new ScientistFieldOfResearchFilter() { FieldOfResearchId = currentDirection.Id });
                return Ok(result);
            }
            catch (Exception e)
            {
                var a = "такого направления нет";
                return Ok(a);
            }
        }

        /// <summary>
        /// Get Scientists for Organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        [HttpGet("GetAllFromWork")]
        public IActionResult GetAllFromWork(string work)
        {
            try
            {
                var currentWork = _workRepository.GetAsync(work);
                var result = _scientistWorkRepository.GetScientistWorkAsync(new ScientistWorkFilter()
                { WorkId = currentWork.Id });
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