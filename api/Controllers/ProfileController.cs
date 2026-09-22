using System.Security.Claims;
using api.Dtos;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var profile = await _profileService.GetAsync(CurrentUserId, cancellationToken);

            return profile is null ? NotFound() : Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto, CancellationToken cancellationToken)
        {
            var profile = await _profileService.SaveAsync(CurrentUserId, dto, cancellationToken);

            return Ok(profile);
        }

        [HttpGet("targets")]
        public async Task<IActionResult> GetNutritionTargets(CancellationToken cancellationToken)
        {
            var calculationDate = DateOnly.FromDateTime(DateTime.UtcNow);

            var targets = await _profileService.GetTargetsAsync(CurrentUserId, calculationDate, cancellationToken);

            return targets is null ? NotFound() : Ok(targets);
        }
    }
}
