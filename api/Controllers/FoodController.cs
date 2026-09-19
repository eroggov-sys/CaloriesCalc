using Microsoft.AspNetCore.Mvc;
using api.Dtos;
using Microsoft.AspNetCore.Authorization;
using api.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
   public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService) => _foodService = foodService;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var food = await _foodService.GetByIdAsync(id, cancellationToken);

            return food is null ? NotFound() : Ok(food);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery]
            [Required]
            [MinLength(2)] 
            string query,
            CancellationToken cancellationToken)
        {
            return Ok(await _foodService.SearchAsync(query, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateFoodDto dto, CancellationToken cancellationToken)
        {
            var food = await _foodService.CreateAsync(dto, cancellationToken);

            if (food is null)
            {
                return Problem(
                    detail: "Food with this name already exists",
                    statusCode: StatusCodes.Status409Conflict);
            }

            return CreatedAtAction(nameof(GetById), new { id = food.Id }, food);
        }
    }
}