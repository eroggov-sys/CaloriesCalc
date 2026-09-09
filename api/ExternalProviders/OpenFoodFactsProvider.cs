using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.OpenFoodFacts;
using api.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using api.Mappers;

namespace api.ExternalProviders
{
    public sealed class OpenFoodFactsProvider : IExternalFoodProvider
    {
        private const int ResultLimit = 10;
        private readonly HttpClient _httpClient;
        public OpenFoodFactsProvider(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<ExternalFoodDto>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var normalizedQuery = query.Trim();

            if (normalizedQuery.Length < 2) return [];

            var parameters = new Dictionary<string, string?>
            {
                ["search_terms"] = normalizedQuery,
                ["search_simple"] = "1",
                ["action"] = "process",
                ["json"] = "1",
                ["page_size"] = ResultLimit.ToString(),
                ["fields"] = "code,product_name,brands,nutriments",
            };
            var requestUri = QueryHelpers.AddQueryString("cgi/search.pl",parameters);

            using var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OpenFoodFactsSearchResponse>(cancellationToken);

            if (result == null) return [];

            return result.Products
                .Select(product => product.ToExternalFoodDto())
                .OfType<ExternalFoodDto>()
                .GroupBy(
                    product => product.ExternalIdentifier,
                    StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .Take(ResultLimit)
                .ToList();
        }
    }
}