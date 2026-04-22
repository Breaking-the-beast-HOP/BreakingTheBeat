using Microsoft.AspNetCore.Mvc;
using BreakingTheBeat.Models;

namespace Gateway.Controllers.Music
{
    [ApiController]
    [Route("[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly ILogger<LanguageController> _logger;

        public LanguageController(ILogger<LanguageController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLanguages")]
        public IEnumerable<LanguageDto> Get()
        {
            var languages = new List<LanguageDto>
            {
                new LanguageDto { Id = 1, Name = "Dansk", ISO = "da-DK" },
                new LanguageDto { Id = 2, Name = "English", ISO = "en-US" },
                new LanguageDto { Id = 3, Name = "Deutsch", ISO = "de-DE" }
            };

            _logger.LogInformation("Languages endpoint was called");

            return languages;
        }
    }
}
