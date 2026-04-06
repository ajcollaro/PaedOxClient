using System.Net.Http.Json;
using PaedOx.Contracts.Analysis;
using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;

namespace PaedOx.Public.Clients;

internal sealed class Analysis(HttpClient http) : IAnalysis
{
    private readonly HttpClient _http = http;

    public async Task<AnalysisDto> Post(OximetryDto dto, CancellationToken token = default)
    {
        using var response = await _http.PostAsJsonAsync("/api/v1/analysis", dto, token);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Analysis failed ({(int)response.StatusCode})");
        }

        return (await response.Content.ReadFromJsonAsync<AnalysisDto>(cancellationToken: token))!;
    }
}
