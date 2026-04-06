using PaedOx.Contracts.Models;

namespace PaedOx.Public.Interfaces;

public interface IModels
{
    Task<ModelsDto> Post(
        ModelsDto Dto,
        CancellationToken Token = default);
}
