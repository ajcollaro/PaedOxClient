using PaedOx.Contracts.Datasets;

namespace PaedOx.Public.Interfaces;

public interface IDatasets
{
    Task<DatasetsDto> Post(
        DatasetsDto Dto,
        CancellationToken Token = default);
}
