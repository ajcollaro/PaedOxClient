using System.Net.Http.Json;
using PaedOx.Contracts.Datasets;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Clients;

internal sealed class Datasets(HttpClient http) : IDatasets
{
    private readonly HttpClient _http = http;

    public async Task<DatasetsDto> Post(DatasetsDto dto, CancellationToken token = default)
    {
        using var response = await _http.PostAsJsonAsync("/api/v1/datasets", dto, token);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Dataset list generation failed ({(int)response.StatusCode})");
        }

        return (await response.Content.ReadFromJsonAsync<DatasetsDto>(cancellationToken: token))!;
    }
}
