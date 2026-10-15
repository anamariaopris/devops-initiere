using OneToManyApi.Dtos;
using OneToManyApi.Models;

namespace OneToManyApi.Repositories;

public interface ICarRepository
{
    Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Car?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Car?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByBrandAsync(string brand, CancellationToken cancellationToken = default);
    Task<List<Car>> GetByYearRangeAsync(int minYear, int maxYear, CancellationToken cancellationToken = default);
    Task<List<Car>> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default);
    Task AddAsync(Car car, CancellationToken cancellationToken = default);
    void Remove(Car car);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
