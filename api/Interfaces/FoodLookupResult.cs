using api.Dtos;

namespace api.Interfaces
{
    public sealed record FoodLookupResult(FoodDto? Food, bool ExternalSearchFailed);
}
