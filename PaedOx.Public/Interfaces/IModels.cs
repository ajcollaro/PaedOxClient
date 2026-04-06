using PaedOx.Contracts.Models;

namespace PaedOx.Public.Interfaces;

public interface IModels
{
    Task<ModelsDto> Post(ModelsDto dto, CancellationToken token = default);
}
