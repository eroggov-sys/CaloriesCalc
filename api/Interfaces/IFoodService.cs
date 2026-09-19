using api.Dtos;

namespace api.Interfaces
{
    public interface IFoodService
    {
        Task<IReadOnlyList<FoodDto>> SearchAsync(string query, CancellationToken cancellationToken = default);

        Task<FoodDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<FoodDto?> CreateAsync(CreateFoodDto dto, CancellationToken cancellationToken = default);
    }
}