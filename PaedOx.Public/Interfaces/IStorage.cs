using PaedOx.Contracts.Storage;

namespace PaedOx.Public.Interfaces;

public interface IStorage
{
    Task<StorageDto> Post(StorageDto dto, CancellationToken token = default);
}
