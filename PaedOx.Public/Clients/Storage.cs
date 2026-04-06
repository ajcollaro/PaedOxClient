using System.Net.Http.Json;
using PaedOx.Contracts.Storage;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Clients;

internal sealed class Storage(HttpClient http) : IStorage
{
    private readonly HttpClient _http = http;

    public async Task<StorageDto> Post(StorageDto dto, CancellationToken token = default)
    {
        using var response = await _http.PostAsJsonAsync("/api/v1/storage", dto, token);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Storage operation failed ({(int)response.StatusCode})");
        }

        return (await response.Content.ReadFromJsonAsync<StorageDto>(cancellationToken: token))!;
    }
}
