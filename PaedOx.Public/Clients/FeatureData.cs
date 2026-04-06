using System.Net.Http.Json;
using PaedOx.Contracts.FeatureData;
using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Clients;

internal sealed class FeatureData(HttpClient http) : IFeatureData
{
    private readonly HttpClient _http = http;

    public async Task<FeatureDataDto> Post(OximetryDto dto, CancellationToken token = default)
    {
        using var response = await _http.PostAsJsonAsync("/api/v1/featuredata", dto, token);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Feature generation failed ({(int)response.StatusCode})");
        }

        return (
            await response.Content.ReadFromJsonAsync<FeatureDataDto>(cancellationToken: token)
        )!;
    }
}
