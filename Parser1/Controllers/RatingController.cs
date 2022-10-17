using Microsoft.AspNetCore.Mvc;
using Parser1.EF;
using Parser1.Interfaces;

namespace Parser1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IMainParser _mainParser;
        private readonly ISupportParser _supportParser;
        private readonly IRatingServise _ratingServise;

        public RatingController(ApplicationContext context, IMainParser mainParser, ISupportParser supportParser, IRatingServise ratingServise)
        {
            _context = context;
            _mainParser = mainParser;
            _supportParser = supportParser;
            _ratingServise = ratingServise;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _ratingServise.GetRatingToAllFromGovUa("Педагогіка");
            return Ok();
        }
    }
}
