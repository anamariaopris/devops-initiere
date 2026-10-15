using OneModelApi.Dtos;

namespace OneModelApi.Services;

public interface ICarCommandService
{
    Task<CarResponse> CreateAsync(CarCreateRequest request, CancellationToken cancellationToken = default);
    Task<CarResponse> UpdateAsync(long id, CarPutRequest request, CancellationToken cancellationToken = default);
    Task<CarResponse> PatchAsync(long id, CarPatchRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
