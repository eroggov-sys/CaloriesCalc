
using api.Dtos;

namespace api.Interfaces
{
    public interface IDiaryService
    {
        Task<DiaryDayDto> GetDayAsync(string userId, DateOnly date, CancellationToken cancellationToken = default);
    }
}