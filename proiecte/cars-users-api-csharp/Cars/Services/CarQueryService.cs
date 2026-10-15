using AutoMapper;
using OneToManyApi.Dtos;
using OneToManyApi.Exceptions;
using OneToManyApi.Repositories;

namespace OneToManyApi.Services;

public class CarQueryService(ICarRepository carRepository, IUserRepository userRepository, IMapper mapper) : ICarQueryService
{
    public async Task<CarListResponse> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cars = await carRepository.GetAllAsync(cancellationToken);
        return mapper.Map<CarListResponse>(cars);
    }

    public async Task<CarResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Car not found with id {id}");

        return mapper.Map<CarResponse>(car);
    }

    public async Task<UserCarsResponse> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException($"User not found with id {userId}");
        }

        var cars = await carRepository.GetByUserIdAsync(userId, cancellationToken);
        var response = mapper.Map<List<CarResponse>>(cars);
        return new UserCarsResponse(userId, response);
    }

    public async Task<CarListResponse> GetByBrandAsync(string brand, CancellationToken cancellationToken = default)
    {
        var cars = await carRepository.GetByBrandAsync(brand, cancellationToken);
        return mapper.Map<CarListResponse>(cars);
    }

    public async Task<CarListResponse> GetByYearRangeAsync(int minYear, int maxYear, CancellationToken cancellationToken = default)
    {
        if (minYear > maxYear)
        {
            throw new BadRequestException($"Invalid year range: minYear ({minYear}) must be <= maxYear ({maxYear})");
        }

        var cars = await carRepository.GetByYearRangeAsync(minYear, maxYear, cancellationToken);
        return mapper.Map<CarListResponse>(cars);
    }

    public async Task<CarListResponse> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default)
    {
        if (request.MinYear is not null && request.MaxYear is not null && request.MinYear > request.MaxYear)
        {
            throw new BadRequestException($"Invalid year range: minYear ({request.MinYear}) must be <= maxYear ({request.MaxYear})");
        }

        var cars = await carRepository.SearchAsync(request, cancellationToken);
        return mapper.Map<CarListResponse>(cars);
    }
}
