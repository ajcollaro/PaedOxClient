using System.Net.Http.Json;
using PaedOx.Contracts.Models;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Clients;

internal sealed class Models(HttpClient http) : IModels
{
    private readonly HttpClient _http = http;

    public async Task<ModelsDto> Post(ModelsDto dto, CancellationToken token = default)
    {
        using var response = await _http.PostAsJsonAsync("/api/v1/models", dto, token);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Model list generation failed ({(int)response.StatusCode})");
        }

        return (await response.Content.ReadFromJsonAsync<ModelsDto>(cancellationToken: token))!;
    }
}
