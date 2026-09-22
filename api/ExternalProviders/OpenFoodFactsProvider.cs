using System.Net;
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

        public async Task<ExternalFoodDto?> GetByBarcodeAsync(string barcode, CancellationToken cancellationToken = default)
        {
            var requestUri = $"api/v2/product/{barcode}.json" +
                "?fields=code,product_name,brands,nutriments";

            using var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<OpenFoodFactsProductResponse>(cancellationToken);

            if (result is null || result.Status != 1 || result.Product is null) return null;

            return result.Product.ToExternalFoodDto();
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