using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class FoodEntryRepository : IFoodEntryRepository
    {
        private readonly AppDbContext _context;
        public FoodEntryRepository (AppDbContext context)
        {
            _context = context;
        }


        public async Task<FoodEntry?> GetByIdAsync(int id, string userId)
        {
            return await _context.FoodEntries
                .Include(entry => entry.Food)
                .FirstOrDefaultAsync(entry => entry.Id == id && entry.UserId == userId);
        }

        public async Task<FoodEntry?> CreateAsync(FoodEntry foodEntryModel)
        {
            await _context.FoodEntries.AddAsync(foodEntryModel);
            await _context.SaveChangesAsync();
            return foodEntryModel;
        }

        public async Task<FoodEntry?> DeleteAsync(int id, string userId)
        {
            var foodEntryModel = await _context.FoodEntries.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (foodEntryModel == null) return null;

            _context.FoodEntries.Remove(foodEntryModel);
            await _context.SaveChangesAsync();

            return foodEntryModel;
        }

        public async Task<List<FoodEntry>> GetAllAsync(string userId)
        {
            return await _context.FoodEntries
                .Include(entry => entry.Food)
                .Where(entry => entry.UserId == userId)
                .ToListAsync();
        }

        public async Task<FoodEntry?> UpdateAsync(int id, FoodEntry foodEntryModel, string userId)
        {
            var existingEntry = await _context.FoodEntries
                .Include(entry => entry.Food)
                .FirstOrDefaultAsync(entry => entry.Id == id && entry.UserId == userId);

            if (existingEntry == null) return null;

            existingEntry.QuantityGrams = foodEntryModel.QuantityGrams;
            
            await _context.SaveChangesAsync();
            return existingEntry;

        }
        public async Task<bool> FoodExistsAsync(int foodId)
        {
            return await _context.Foods.AnyAsync(food => food.Id == foodId);
        }

        public async Task<List<FoodEntry>> GetByDateAsync(DateOnly date, string userId)
        {
            var start = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            var end = start.AddDays(1);

            return await _context.FoodEntries
                          .Include(entry => entry.Food)
                          .Where(entry => 
                            entry.UserId == userId &&
                            entry.EatenAt >= start && 
                            entry.EatenAt < end)
                          .ToListAsync();
        }

        
    }
}