using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FoodService : IFoodService
    {
        private const int SearchResultLimit = 20;

        private readonly AppDbContext _context;

        public FoodService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<FoodDto>> SearchAsync(
            string query,
            CancellationToken cancellationToken = default)
        {
            var searchQuery = query.Trim();

            var foods = await _context.Foods
                .AsNoTracking()
                .Where(food => EF.Functions.ILike(food.Name, $"%{searchQuery}%"))
                .OrderBy(food => food.Name)
                .Take(SearchResultLimit)
                .ToListAsync(cancellationToken);

            return foods
                .Select(food => food.ToFoodDto())
                .ToList();
        }

        public async Task<FoodDto?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var food = await _context.Foods
                .AsNoTracking()
                .FirstOrDefaultAsync(food => food.Id == id, cancellationToken);

            return food?.ToFoodDto();
        }

        public async Task<FoodDto?> CreateAsync(
            CreateFoodDto dto,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = dto.Name.Trim();

            var foodExists = await _context.Foods
                .AsNoTracking()
                .AnyAsync(
                    food => EF.Functions.ILike(food.Name, normalizedName),
                    cancellationToken);

            if (foodExists) return null;

            var foodModel = dto.ToFoodFromCreate();

            _context.Foods.Add(foodModel);
            await _context.SaveChangesAsync(cancellationToken);

            return foodModel.ToFoodDto();
        }
    }
}
