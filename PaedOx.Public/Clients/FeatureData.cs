using PaedOx.Contracts.FeatureData;
using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;
using System.Net.Http.Json;

namespace PaedOx.Public.Clients;

internal sealed class FeatureData(HttpClient http) : IFeatureData
{
    private readonly HttpClient _http = http;

    public async Task<FeatureDataDto> Post(OximetryDto Dto, CancellationToken Token = default)
    {
        using var Response = await _http.PostAsJsonAsync("/api/v1/featuredata", Dto, Token);

        if (!Response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Feature generation failed ({(int)Response.StatusCode})");
        }

        return (await Response.Content.ReadFromJsonAsync<FeatureDataDto>(cancellationToken: Token))!;
    }
}
