using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using api.Interfaces;

namespace api.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]    
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly INutritionCalculator _nutritionCalculator;        
        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        public ProfileController(AppDbContext context, INutritionCalculator nutritionCalculator)
        {
            _context = context;
            _nutritionCalculator = nutritionCalculator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _context.UserProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(profile => profile.UserId == CurrentUserId);
            if ( profile == null) return NotFound();

            return Ok(profile.ToUserProfileDto());
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (dto.DateOfBirth >= today) 
                return BadRequest("Date of birth must be  past");

            var profile = await _context.UserProfiles.FirstOrDefaultAsync(profile => profile.UserId == CurrentUserId);
        
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = CurrentUserId
                };
                profile.UpdateFromDto(dto);
                _context.UserProfiles.Add(profile);
            }
            else
            {
                profile.UpdateFromDto(dto);
            }
            await _context.SaveChangesAsync();

            return Ok(profile.ToUserProfileDto());
        
        }

    [HttpGet("targets")]
    public async Task<IActionResult> GetNutritionTargets()
    {
        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(profile => profile.UserId == CurrentUserId);
        
        if (profile == null) return NotFound("User profile has not been created");

        var calculationDate = DateOnly.FromDateTime(DateTime.UtcNow);

        var targets = _nutritionCalculator.Calculate(profile, calculationDate);

        return Ok(targets);
    }

    }
    
}