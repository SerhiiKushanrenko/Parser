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
        public async Task<ActionResult<IEnumerable<Scientist>>> GetAllFromDirection(string fieldOfResearch)
        {

            try
            {
                var currentDirection = await _fieldOfResearchRepository.GetAsync(fieldOfResearch);
                var getScientistFieldOfResearches = await _scientistFieldOfResearchRepository.GetScientistsFieldsOfResearchAsync(new ScientistFieldOfResearchFilter() { FieldOfResearchId = currentDirection.Id });
                var result = await _scientistRepository.GetAllFromFieldOfResearch(getScientistFieldOfResearches);

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
        public async Task<ActionResult<IEnumerable<Scientist>>> GetAllFromWork(string work)
        {
            try
            {
                var currentWork = await _workRepository.GetAsync(work);
                var getScientistWork = await _scientistWorkRepository.GetScientistWorkAsync(new ScientistWorkFilter()
                { WorkId = currentWork.Id });
                var result = await _scientistRepository.GetAllFromWork(getScientistWork);
                return Ok(result);
            }
            catch (Exception e)
            {
                var a = "Такой работы нет";
                return Ok(a);
            }
        }
    }
}