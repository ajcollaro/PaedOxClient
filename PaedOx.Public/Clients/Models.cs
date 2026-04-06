using PaedOx.Contracts.Models;
using PaedOx.Public.Interfaces;
using System.Net.Http.Json;

namespace PaedOx.Public.Clients;

internal sealed class Models(HttpClient http) : IModels
{
    private readonly HttpClient _http = http;

    public async Task<ModelsDto> Post(ModelsDto Dto, CancellationToken Token = default)
    {
        using var Response = await _http.PostAsJsonAsync("/api/v1/models", Dto, Token);

        if (!Response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Model list generation failed ({(int)Response.StatusCode})");
        }

        return (await Response.Content.ReadFromJsonAsync<ModelsDto>(cancellationToken: Token))!;
    }
}
