using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Mappers;
using api.Dtos;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class FoodEntriesController : ControllerBase
    {
        private readonly IFoodEntryRepository _foodEntryRepo;
        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public FoodEntriesController(IFoodEntryRepository foodEntryRepo)
        {
            _foodEntryRepo = foodEntryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var entries = await _foodEntryRepo.GetAllAsync(CurrentUserId);
            var foodEntryDto = entries.Select(s => s.ToFoodEntryDto());
            
            return Ok(foodEntryDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var foodEntry = await _foodEntryRepo.GetByIdAsync(id, CurrentUserId);

            if(foodEntry == null) return NotFound();

            return Ok(foodEntry.ToFoodEntryDto());
        }



        [HttpPost("{foodId}")]
        public async Task<IActionResult> Create([FromRoute] int foodId,[FromBody] CreateEntryFoodDto entryFoodDto)
        {   
            var foodExists = await _foodEntryRepo.FoodExistsAsync(foodId);

            if (!foodExists)
                return BadRequest("Food does not exist");

            var foodEntryModel = entryFoodDto.ToFoodEntryFromCreate(foodId, CurrentUserId);

            await _foodEntryRepo.CreateAsync(foodEntryModel);

            var createdEntry = await _foodEntryRepo.GetByIdAsync(foodEntryModel.Id, CurrentUserId);

            return CreatedAtAction(nameof(GetById), new { id = foodEntryModel.Id }, createdEntry!.ToFoodEntryDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateFoodEntryRequestDto updateDto)
        {
            var foodEntryModel = await _foodEntryRepo.UpdateAsync(id, updateDto.ToFoodEntrytFromUpdate(), CurrentUserId);
            if(foodEntryModel == null) return NotFound("Food entry not found");

            return Ok(foodEntryModel.ToFoodEntryDto());
        }

        [HttpDelete]
        [Route("{id}")]
        
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentModel = await _foodEntryRepo.DeleteAsync(id, CurrentUserId);

            if (commentModel == null) return NotFound();

            return NoContent();
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily([FromQuery] DateOnly? date)
        {
            if (date == null)  return BadRequest("Date is required");

            var entries = await _foodEntryRepo.GetByDateAsync(date.Value, CurrentUserId);
            
            var entryDtos = entries
                             .Select(entry => entry.ToFoodEntryDto())
                             .ToList();

            var result = new DailyNutritionDto
            {
                Date = date.Value,
                Calories = entryDtos.Sum(entry => entry.Calories),
                Protein = entryDtos.Sum(entry => entry.Protein),
                Fat = entryDtos.Sum(entry => entry.Fat),
                Carbs = entryDtos.Sum(entry => entry.Carbs),
                Sugar = entryDtos.Sum(entry => entry.Sugar)
            };

            return Ok(result);
        }

        [HttpGet("by-date")]
        public async Task<IActionResult> GetByDategGroupedAsync([FromQuery] DateOnly? date)
        {
            if (date == null) return BadRequest("Date is required");

            var entries = await _foodEntryRepo.GetByDateAsync(date.Value, CurrentUserId);
            var entryDtos = entries
                             .Select(entry => entry.ToFoodEntryDto())
                             .ToList();
            var groups = entryDtos
                          .GroupBy(entry => entry.MealType)
                          .Select(group => new MealGroupDto
                          {
                                MealType = group.Key,
                                TotalCalories = group.Sum(entry => entry.Calories),
                                Entries = group.ToList(),
                          }).ToList();

            return Ok(groups);
        }
    
    
    
    }


}