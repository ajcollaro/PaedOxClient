using PaedOx.Contracts.Datasets;
using PaedOx.Public.Interfaces;
using System.Net.Http.Json;

namespace PaedOx.Public.Clients;

internal sealed class Datasets(HttpClient http) : IDatasets
{
    private readonly HttpClient _http = http;

    public async Task<DatasetsDto> Post(DatasetsDto Dto, CancellationToken Token = default)
    {
        using var Response = await _http.PostAsJsonAsync("/api/v1/datasets", Dto, Token);

        if (!Response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Dataset list generation failed ({(int)Response.StatusCode})");
        }

        return (await Response.Content.ReadFromJsonAsync<DatasetsDto>(cancellationToken: Token))!;
    }
}
