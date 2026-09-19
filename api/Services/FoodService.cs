using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FoodService : IFoodService
    {
        private const int SearchResultLimit = 20;

        private readonly AppDbContext _context;
        private readonly IExternalFoodProvider _externalFoodProvider;
        private readonly ILogger<FoodService> _logger;

        public FoodService(AppDbContext context, IExternalFoodProvider externalFoodProvider, ILogger<FoodService> logger)
        {
            _context = context;
            _externalFoodProvider = externalFoodProvider;
            _logger = logger;
        }

        public async Task<FoodSearchResult> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var searchQuery = query.Trim();

            var localFoods = await SearchLocalAsync(searchQuery, cancellationToken);

            if (localFoods.Count > 0)
            {
                return new FoodSearchResult(localFoods, ExternalSearchFailed: false);
            }

            return await ImportFromExternalAsync(searchQuery, cancellationToken);
        }

        private async Task<IReadOnlyList<FoodDto>> SearchLocalAsync(string query, CancellationToken cancellationToken = default)
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

        private async Task<FoodSearchResult> ImportFromExternalAsync(
        string searchQuery, CancellationToken cancellationToken)
        {
            IReadOnlyList<ExternalFoodDto> externalFoods;

            try
            {
                externalFoods = await _externalFoodProvider.SearchAsync(searchQuery, cancellationToken);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(exception,
                    "External food search failed for query {Query}", searchQuery);

                return new FoodSearchResult([], ExternalSearchFailed: true);
            }

            if (externalFoods.Count == 0)
            {
                return new FoodSearchResult([], ExternalSearchFailed: false);
            }

            var externalIds = externalFoods
                .Select(food => food.ExternalIdentifier)
                .ToList();

            var knownIds = await _context.Foods
                .Where(food =>
                    food.Source == FoodSource.OpenFoodFacts &&
                    food.ExternalId != null &&
                    externalIds.Contains(food.ExternalId))
                .Select(food => food.ExternalId!)
                .ToListAsync(cancellationToken);

            var newFoods = externalFoods
                .Where(food => !knownIds.Contains(food.ExternalIdentifier))
                .Select(food => food.ToFood())
                .ToList();

            if (newFoods.Count > 0)
            {
                _context.Foods.AddRange(newFoods);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var foods = await SearchLocalAsync(searchQuery, cancellationToken);

            return new FoodSearchResult(foods, ExternalSearchFailed: false);
        }



        public async Task<FoodDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var food = await _context.Foods
                .AsNoTracking()
                .FirstOrDefaultAsync(food => food.Id == id, cancellationToken);

            return food?.ToFoodDto();
        }

        public async Task<FoodDto?> CreateAsync( CreateFoodDto dto, CancellationToken cancellationToken = default)
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
