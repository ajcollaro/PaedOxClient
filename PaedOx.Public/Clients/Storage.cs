using PaedOx.Contracts.Storage;
using PaedOx.Public.Interfaces;
using System.Net.Http.Json;

namespace PaedOx.Public.Clients;

internal sealed class Storage(HttpClient http) : IStorage
{
    private readonly HttpClient _http = http;

    public async Task<StorageDto> Post(StorageDto Dto, CancellationToken Token = default)
    {
        using var Response = await _http.PostAsJsonAsync("/api/v1/storage", Dto, Token);

        if (!Response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Storage operation failed ({(int)Response.StatusCode})");
        }

        return (await Response.Content.ReadFromJsonAsync<StorageDto>(cancellationToken: Token))!;
    }
}
