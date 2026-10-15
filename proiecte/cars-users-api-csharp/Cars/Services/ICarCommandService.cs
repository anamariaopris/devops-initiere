using OneToManyApi.Dtos;

namespace OneToManyApi.Services;

public interface ICarCommandService
{
    Task<CarResponse> CreateAsync(CarCreateRequest request, CancellationToken cancellationToken = default);
    Task<CarResponse> UpdateAsync(long id, CarUpdateRequest request, CancellationToken cancellationToken = default);
    Task<CarResponse> PatchAsync(long id, CarPatchRequest request, CancellationToken cancellationToken = default);
    Task<CarResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
