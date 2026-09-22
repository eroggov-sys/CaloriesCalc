using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Mappers;
using api.Dtos;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class FoodEntriesController : ControllerBase
    {
        private readonly IFoodEntryService _foodEntryService;

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public FoodEntriesController(IFoodEntryService foodEntryService)
        {
            _foodEntryService = foodEntryService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var foodEntry = await _foodEntryService.GetByIdAsync(id, CurrentUserId, cancellationToken);

            if(foodEntry == null) return NotFound();

            return Ok(foodEntry);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFoodEntryDto entryFoodDto, CancellationToken cancellationToken)
        {   

            var entry = await _foodEntryService.CreateAsync(CurrentUserId, entryFoodDto, cancellationToken);

            if (entry == null)
            {
                ModelState.AddModelError(nameof(entryFoodDto.FoodId), "Food does not exist");
                return ValidationProblem(ModelState);
            }
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, entry);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateFoodEntryDto updateDto, CancellationToken cancellationToken)
        {
            var entry = await _foodEntryService.UpdateAsync(id, CurrentUserId, updateDto,  cancellationToken);
            if(entry == null) return NotFound();

            return Ok(entry);
        }

        [HttpDelete]
        [Route("{id:int}")]
        
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var deleted = await _foodEntryService.DeleteAsync(id, CurrentUserId, cancellationToken);

            if (deleted == false) return NotFound();

            return NoContent();
        }
    
    }


}