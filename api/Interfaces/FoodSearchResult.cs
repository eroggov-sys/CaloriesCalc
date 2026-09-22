using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;

namespace api.Interfaces
{
        public sealed record FoodSearchResult(IReadOnlyList<FoodDto> Foods, bool HasMore, bool ExternalSearchFailed);

}