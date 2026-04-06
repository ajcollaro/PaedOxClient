using PaedOx.Contracts.Analysis;
using PaedOx.Contracts.Oximetry;

namespace PaedOx.Public.Interfaces;

public interface IAnalysis
{
    Task<AnalysisDto> Post(OximetryDto dto, CancellationToken token = default);
}
