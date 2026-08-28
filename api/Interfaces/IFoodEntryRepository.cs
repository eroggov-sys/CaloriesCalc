using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IFoodEntryRepository
    {
        Task<List<FoodEntry>> GetAllAsync(string userId);
        Task<FoodEntry?> GetByIdAsync(int id, string userId);
        Task<FoodEntry?> CreateAsync(FoodEntry foodEntry);
        Task<FoodEntry?> DeleteAsync(int id, string userId);
        Task<FoodEntry?> UpdateAsync(int id, FoodEntry foodEntry, string userId);
        Task<bool> FoodExistsAsync(int foodId);
        Task<List<FoodEntry>> GetByDateAsync(DateOnly date, string userId);

    }
}