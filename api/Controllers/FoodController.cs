using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Dtos;
using api.Mappers;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FoodController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var foods = await _context.Foods.ToListAsync();
            return Ok(foods);
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateFoodDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var foodModel = dto.ToFoodFromCreate();

            var normalizedName = dto.Name.Trim();

            var foodExists = await _context.Foods
                .AnyAsync(food =>
                    EF.Functions.ILike(food.Name, normalizedName));

            if (foodExists)
            {
                return Conflict("Food already exists");
            }

            _context.Foods.Add(foodModel);
            await _context.SaveChangesAsync();

            return Ok(foodModel);
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? query)
        {

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required");
            }

            var searchQuery = query.Trim();

            var foods = await _context.Foods
                .Where(food => EF.Functions.ILike(food.Name, $"%{searchQuery}%"))
                .OrderBy(food => food.Name)
                .Take(20)
                .ToListAsync();
            
            return Ok(foods);


        }
    }
}