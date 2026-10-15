using OneModelApi.Dtos;
using OneModelApi.Models;

namespace OneModelApi.Repositories;

public interface ICarRepository
{
    Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Car?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Car?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByBrandAsync(string brand, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByColorAsync(string color, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByFuelTypeAsync(string fuelType, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByTransmissionAsync(string transmission, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByAvailableAsync(bool available, CancellationToken cancellationToken = default);
    Task<List<Car>> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default);
    Task<bool> ExistsByBrandModelAsync(
        string brand,
        string model,
        long? excludedId = null,
        CancellationToken cancellationToken = default);
    Task AddAsync(Car car, CancellationToken cancellationToken = default);
    Task DeleteAsync(Car car, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
