using System.Net.Http.Json;
using Order.Core.Services.Dtos;
using Order.Core.Services.Http;
using Product.Core.Entities;

namespace Order.Infra.Services;

public class ProductServices(string productUrl) : IProductService
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri(productUrl) };

    public async Task<IEnumerable<ProductDto>> GetProducts(IEnumerable<Guid> ids)
    {
        var request = await _client.GetAsync($"api/products/custom?ids={string.Join(",", ids)},");
        if (request.IsSuccessStatusCode)
        {
            return (await request.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>()) ?? [];
        }

        return [];
    }
}