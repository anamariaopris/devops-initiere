using OneToManyApi.Dtos;

namespace OneToManyApi.Services;

public interface ICarQueryService
{
    Task<CarListResponse> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<UserCarsResponse> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByBrandAsync(string brand, CancellationToken cancellationToken = default);
    Task<CarListResponse> GetByYearRangeAsync(int minYear, int maxYear, CancellationToken cancellationToken = default);
    Task<CarListResponse> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default);
}
