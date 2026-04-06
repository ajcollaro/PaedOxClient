using PaedOx.Contracts.Datasets;

namespace PaedOx.Public.Interfaces;

public interface IDatasets
{
    Task<DatasetsDto> Post(DatasetsDto dto, CancellationToken token = default);
}
