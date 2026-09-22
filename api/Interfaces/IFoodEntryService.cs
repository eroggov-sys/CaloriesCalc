using api.Dtos;

namespace api.Interfaces
{
    public interface IFoodEntryService 
    {
        Task<FoodEntryDto?> GetByIdAsync(int id, string userId, CancellationToken cancellationToken = default);

        Task<FoodEntryDto?> CreateAsync(string userId, CreateFoodEntryDto dto, CancellationToken cancellationToken = default);

        Task<FoodEntryDto?> UpdateAsync(int id, string userId, UpdateFoodEntryDto dto, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, string userId, CancellationToken cancellationToken = default);
    
    }
}