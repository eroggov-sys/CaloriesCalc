using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class DiaryService : IDiaryService
    {
        private readonly AppDbContext _context;

        public DiaryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DiaryDayDto> GetDayAsync(string userId, DateOnly date, CancellationToken cancellationToken = default)
        {
            var entries = await _context.FoodEntries
                .AsNoTracking()
                .Include(entry => entry.Food)
                .Where(entry => entry.UserId == userId && entry.Date == date)
                .OrderBy(entry => entry.Id)
                .ToListAsync(cancellationToken);
            
            var meals = Enum.GetValues<MealType>()
                .Select(mealType =>
                {
                    var mealEntries = entries
                        .Where(entry => entry.MealType == mealType)
                        .ToList();

                    return new MealGroupDto
                    {
                        MealType = mealType,
                        Totals = mealEntries.ToTotalsDto(),
                        Entries = mealEntries.Select(entry => entry.ToFoodEntryDto()).ToList(),
                    };
                })
                .ToList();
            
            return new DiaryDayDto
            {
                Date = date,
                Totals = entries.ToTotalsDto(),
                Meals = meals,
            };
        }
    }
}