using APIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DIController : Controller
    {
        private readonly ITransientService _transientService;
        private readonly IScopedService _scopedService;
        private readonly ISingletonService _singletonService;
        private readonly DIReporterService _reporterService;

        public DIController(ITransientService transientService, IScopedService scopedService, ISingletonService singletonService, DIReporterService reporterService)
        {
            _transientService = transientService;
            _scopedService = scopedService;
            _singletonService = singletonService;
            _reporterService = reporterService;
        }
        [HttpGet]
        public IActionResult CheckLifetimes()
        {
            return Ok(new
            {
                TransientCoparison = new
                {
                    ControllerInstance = _transientService.Id,
                    ReposrterInstance = _reporterService.TransientService.Id,
                    IsSame = _transientService.Id == _reporterService.TransientService.Id
                },
                ScopedComparison = new
                {
                    ControllerInstance = _scopedService.Id,
                    ReposrterInstance = _reporterService.ScopedService.Id,
                    IsSame = _scopedService.Id == _reporterService.ScopedService.Id
                },
                SingletonComparison = new
                {
                    ControllerInstance = _singletonService.Id,
                    ReposrterInstance = _reporterService.SingletonService.Id,
                    IsSame = _singletonService.Id == _reporterService.SingletonService.Id
                }
            });
        }
    }
}
