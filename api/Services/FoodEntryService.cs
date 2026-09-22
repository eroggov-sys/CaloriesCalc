using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FoodEntryService : IFoodEntryService
    {
        private readonly AppDbContext _context;
        public FoodEntryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FoodEntryDto?> CreateAsync(string userId, CreateFoodEntryDto dto, CancellationToken cancellationToken = default)
        {
            var food = await _context.Foods
                .FirstOrDefaultAsync(food => food.Id == dto.FoodId, cancellationToken);

            if (food is null) return null;

            var entry = dto.ToFoodEntryFromCreate(userId);
            entry.Food = food;

            _context.FoodEntries.Add(entry);
            await _context.SaveChangesAsync(cancellationToken);

            return entry.ToFoodEntryDto();
        }

        public async Task<bool> DeleteAsync(int id, string userId, CancellationToken cancellationToken = default)
        {
            var deletedCount = await _context.FoodEntries
                .Where(entry => entry.Id == id && entry.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            return deletedCount > 0;
        }

        public async Task<FoodEntryDto?> GetByIdAsync(int id, string userId, CancellationToken cancellationToken = default)
        {
            var entry = await _context.FoodEntries
                .AsNoTracking()
                .Include(entry => entry.Food)
                .FirstOrDefaultAsync(
                    entry => entry.Id == id && entry.UserId == userId,
                    cancellationToken);

            return entry?.ToFoodEntryDto();
        }

        public async Task<FoodEntryDto?> UpdateAsync(int id, string userId, UpdateFoodEntryDto dto, CancellationToken cancellationToken = default)
        {
            var entry = await _context.FoodEntries
                .Include(entry => entry.Food)
                .FirstOrDefaultAsync(
                    entry => entry.Id == id && entry.UserId == userId,
                    cancellationToken);

            if (entry is null) return null;

            entry.QuantityGrams = dto.QuantityGrams;

            await _context.SaveChangesAsync(cancellationToken);

            return entry.ToFoodEntryDto();
        }
    }
}