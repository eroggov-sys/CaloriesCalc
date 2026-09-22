using api.Dtos;

namespace api.Interfaces
{
    public interface IFoodService
    {
        Task<FoodSearchResult> SearchAsync(string query, int page, int pageSize, bool includeExternal, CancellationToken cancellationToken = default);

        Task<FoodDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<FoodDto?> CreateAsync(CreateFoodDto dto, CancellationToken cancellationToken = default);
        Task<FoodLookupResult> ImportAsync(ImportFoodDto dto, CancellationToken cancellationToken = default);

        
    }
}