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
            [FromQuery][Required][MinLength(2, ErrorMessage = "Search query must be at least 2 characters")] string query,
            [FromQuery] bool external = false,
            [FromQuery][Range(1, 1000)] int page = 1,
            [FromQuery][Range(1, 50)] int pageSize = 20,
            CancellationToken cancellationToken = default)

        {
            var result = await _foodService.SearchAsync(query, page, pageSize, external,cancellationToken);  

            if (result.ExternalSearchFailed && result.Foods.Count == 0)
            {
                return Problem(
                    detail: "Food database is temporarily unavailable, please try again later",
                    statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            return Ok(new PagedResponseDto<FoodDto>
            {
                Items = result.Foods,
                Page = page,
                PageSize = pageSize,
                HasMore = result.HasMore,
            });
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

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] ImportFoodDto dto, CancellationToken cancellationToken)
        {
            var result = await _foodService.ImportAsync(dto, cancellationToken);

            if (result.ExternalSearchFailed)
            {
                return Problem(
                    detail: "Food database is temporarily unavailable, please try again later",
                    statusCode: StatusCodes.Status503ServiceUnavailable);
            }
            
            if (result.Food == null) return NotFound();

            return Ok(result.Food);
        }

        [HttpGet("barcode/{barcode}")]
        public async Task<IActionResult> GetByBarcode(
            [RegularExpression(@"^\d{8,14}$", ErrorMessage = "Barcode must contain 8 to 14 digits")] string barcode,
            CancellationToken cancellationToken)
        {
            var result = await _foodService.FindByBarcodeAsync(barcode, cancellationToken);

            if (result.ExternalSearchFailed)
            {
                return Problem(
                    detail: "Food database is temporarily unavailable, please try again later",
                    statusCode: StatusCodes.Status503ServiceUnavailable);
            }

            if (result.Food == null) return NotFound();

            return Ok(result.Food);
        }


    }
}