using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/diary")]
    public class DiaryController : ControllerBase
    {
        private readonly IDiaryService _diaryService;

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public DiaryController(IDiaryService diaryService)
        {
            _diaryService = diaryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDay([FromQuery][Required] DateOnly? date, CancellationToken cancellationToken)
        {
            var day = await _diaryService.GetDayAsync(CurrentUserId, date!.Value, cancellationToken);

            return Ok(day);
        }
    }
}
