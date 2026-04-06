using PaedOx.Contracts.Analysis;
using PaedOx.Contracts.Oximetry;
using PaedOx.Public.Interfaces;
using System.Net.Http.Json;

namespace PaedOx.Public.Clients;

internal sealed class Analysis(HttpClient http) : IAnalysis
{
    private readonly HttpClient _http = http;

    public async Task<AnalysisDto> Post(OximetryDto Dto, CancellationToken Token = default)
    {
        using var Response = await _http.PostAsJsonAsync("/api/v1/analysis", Dto, Token);

        if (!Response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Analysis failed ({(int)Response.StatusCode})");
        }

        return (await Response.Content.ReadFromJsonAsync<AnalysisDto>(cancellationToken: Token))!;
    }
}
