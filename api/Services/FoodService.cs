using api.Data;
using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FoodService : IFoodService
    {

        private readonly AppDbContext _context;
        private readonly IExternalFoodProvider _externalFoodProvider;
        private readonly ILogger<FoodService> _logger;

        public FoodService(AppDbContext context, IExternalFoodProvider externalFoodProvider, ILogger<FoodService> logger)
        {
            _context = context;
            _externalFoodProvider = externalFoodProvider;
            _logger = logger;
        }
        
        private static string EscapeLikePattern(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace("%", "\\%")
                .Replace("_", "\\_");
        }

        public async Task<FoodSearchResult> SearchAsync(string query, int page, int pageSize, bool includeExternal, CancellationToken cancellationToken = default)
        {
            var searchQuery = query.Trim();
            var skip = (page - 1) * pageSize;

            var (localFoods, hasMore) = await SearchLocalAsync(searchQuery, skip, pageSize, cancellationToken);

            if (!includeExternal || page > 1)
            {
                return new FoodSearchResult(localFoods, hasMore, ExternalSearchFailed: false);
            }

            return await SearchExternalAsync(searchQuery, localFoods, cancellationToken);
        }

        private async Task<(IReadOnlyList<FoodDto> Foods, bool HasMore)> SearchLocalAsync(string searchQuery, int skip, int take, CancellationToken cancellationToken = default)
        {
            var escaped = EscapeLikePattern(searchQuery);
            var contains = $"%{escaped}%";
            var startsWith = $"{escaped}%";
            var useFuzzySearch = searchQuery.Length >= 4;


            var foods = await _context.Foods
                .AsNoTracking()
                .Where(food => food.Barcode == searchQuery ||
                    EF.Functions.ILike(food.Name, contains) ||
                    (food.Brand != null && EF.Functions.ILike(food.Brand, contains)) ||
                    (useFuzzySearch && EF.Functions.TrigramsAreWordSimilar(searchQuery, food.Name)))
                .OrderBy(food =>
                    food.Barcode == searchQuery ? 0 :
                    EF.Functions.ILike(food.Name, escaped) ? 1 :
                    EF.Functions.ILike(food.Name, startsWith) ? 2 :
                    EF.Functions.ILike(food.Name, contains) ? 3 :
                    food.Brand != null && EF.Functions.ILike(food.Brand, contains) ? 4 :
                    5)
                .ThenByDescending(food => EF.Functions.TrigramsWordSimilarity(searchQuery, food.Name)) 
                .ThenBy(food => food.Name.Length)
                .ThenBy(food => food.Name)
                .ThenBy(food => food.Id) 
                .Skip(skip)   
                .Take(take + 1)
                .ToListAsync(cancellationToken);

                var hasMore = foods.Count > take;

                var page = foods
                    .Take(take)
                    .Select(food => food.ToFoodDto())
                    .ToList();


                return (page, hasMore);
        }

        private async Task<FoodSearchResult> SearchExternalAsync(string searchQuery, IReadOnlyList<FoodDto> localFoods, CancellationToken cancellationToken)
        {
            if (searchQuery.Count(char.IsLetterOrDigit) < 2)
            {
                return new FoodSearchResult(localFoods, HasMore: false, ExternalSearchFailed: false);
            }

            IReadOnlyList<ExternalFoodDto> externalFoods;

            try
            {
                externalFoods = await _externalFoodProvider.SearchAsync(searchQuery, cancellationToken);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(exception,
                    "External food search failed for query {Query}", searchQuery);

                return new FoodSearchResult(localFoods, HasMore: false, ExternalSearchFailed: true);
            }

            if (externalFoods.Count == 0)
            {
                return new FoodSearchResult(localFoods, HasMore: false, ExternalSearchFailed: false);
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

            var candidates = externalFoods
                .Where(food => !knownIds.Contains(food.ExternalIdentifier))
                .Select(food => food.ToCandidateDto())
                .ToList();

            return new FoodSearchResult(
                [.. localFoods, .. candidates],
                HasMore: false,
                ExternalSearchFailed: false);
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
                .AnyAsync(food => EF.Functions.ILike(food.Name, EscapeLikePattern(normalizedName)), cancellationToken);

            if (foodExists) return null;

            var foodModel = dto.ToFoodFromCreate();

            _context.Foods.Add(foodModel);
            await _context.SaveChangesAsync(cancellationToken);

            return foodModel.ToFoodDto();
        }

        public async Task<FoodLookupResult> ImportAsync(ImportFoodDto dto, CancellationToken cancellationToken = default)
        {
            var existingFood = await _context.Foods
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    food => food.Source == dto.Source && food.ExternalId == dto.ExternalId,
                    cancellationToken);

            if (existingFood is not null)
            {
                return new FoodLookupResult(existingFood.ToFoodDto(), ExternalSearchFailed: false);
            }

            ExternalFoodDto? externalFood;

            try
            {
                externalFood = await _externalFoodProvider.GetByBarcodeAsync(dto.ExternalId, cancellationToken);
            }
            catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(exception, "Food import failed for {ExternalId}", dto.ExternalId);

                return new FoodLookupResult(null, ExternalSearchFailed: true);
            }

            if (externalFood is null)
            {
                return new FoodLookupResult(null, ExternalSearchFailed: false);
            }

            var food = externalFood.ToFood();

            _context.Foods.Add(food);
            await _context.SaveChangesAsync(cancellationToken);

            return new FoodLookupResult(food.ToFoodDto(), ExternalSearchFailed: false);

                
        }
    }
}
