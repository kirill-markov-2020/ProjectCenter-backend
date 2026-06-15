using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrivacyController : ControllerBase
    {
        private readonly IPrivacyService _privacyService;

        public PrivacyController(IPrivacyService privacyService)
        {
            _privacyService = privacyService;
        }

        [HttpGet("data-storage-summary")]
        public async Task<IActionResult> GetDataStorageSummary()
        {
            var result = await _privacyService.GetDataStorageSummaryAsync();
            return Ok(result);
        }
    }
}
