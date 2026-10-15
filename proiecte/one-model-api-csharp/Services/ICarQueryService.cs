using OneModelApi.Dtos;

namespace OneModelApi.Services;

public interface ICarQueryService
{
    Task<CarListResponse> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByBrandAsync(string brand, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByColorAsync(string color, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByFuelTypeAsync(string fuelType, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByTransmissionAsync(string transmission, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByAvailableAsync(bool available, CancellationToken cancellationToken = default);
    Task<CarListResponse> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default);
}
