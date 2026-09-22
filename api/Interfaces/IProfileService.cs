using api.Dtos;

namespace api.Interfaces
{
    public interface IProfileService
    {
        Task<UserProfileDto?> GetAsync(string userId, CancellationToken cancellationToken = default);

        Task<UserProfileDto> SaveAsync(string userId, UpdateUserProfileDto dto, CancellationToken cancellationToken = default);

        Task<NutritionTargetDto?> GetTargetsAsync(string userId, DateOnly calculationDate, CancellationToken cancellationToken = default);
    
    }
}