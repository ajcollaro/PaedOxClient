using PaedOx.Contracts.FeatureData;
using PaedOx.Contracts.Oximetry;

namespace PaedOx.Public.Interfaces;

public interface IFeatureData
{
    Task<FeatureDataDto> Post(OximetryDto dto, CancellationToken token = default);
}
