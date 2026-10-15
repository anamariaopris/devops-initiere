using OneModelApi.Dtos;
using OneModelApi.Exceptions;
using OneModelApi.Repositories;

namespace OneModelApi.Services;

public class CarQueryService(ICarRepository carRepository) : ICarQueryService
{
    public async Task<CarListResponse> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cars = await carRepository.GetAllAsync(cancellationToken);

        return new CarListResponse(cars.Select(CarMapper.ToResponse));
    }

    public async Task<CarResponse> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new CarNotFoundException(id);

        return CarMapper.ToResponse(car);
    }

    public async Task<CarListResponse> GetByBrandAsync(string brand, CancellationToken cancellationToken = default)
        => new((await carRepository.GetByBrandAsync(brand, cancellationToken)).Select(CarMapper.ToResponse));

    public async Task<CarListResponse> GetByColorAsync(string color, CancellationToken cancellationToken = default)
        => new((await carRepository.GetByColorAsync(color, cancellationToken)).Select(CarMapper.ToResponse));

    public async Task<CarListResponse> GetByFuelTypeAsync(string fuelType, CancellationToken cancellationToken = default)
        => new((await carRepository.GetByFuelTypeAsync(fuelType, cancellationToken)).Select(CarMapper.ToResponse));

    public async Task<CarListResponse> GetByTransmissionAsync(string transmission, CancellationToken cancellationToken = default)
        => new((await carRepository.GetByTransmissionAsync(transmission, cancellationToken)).Select(CarMapper.ToResponse));

    public async Task<CarListResponse> GetByAvailableAsync(bool available, CancellationToken cancellationToken = default)
        => new((await carRepository.GetByAvailableAsync(available, cancellationToken)).Select(CarMapper.ToResponse));

    public async Task<CarListResponse> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default)
    {
        if (request.MinYear is not null && request.MaxYear is not null && request.MinYear > request.MaxYear)
        {
            throw new InvalidYearRangeException(request.MinYear.Value, request.MaxYear.Value);
        }

        if (request.MaxPrice is not null && request.MaxPrice < 0)
        {
            throw new InvalidSearchFilterException("maxPrice must be >= 0");
        }

        if (request.MaxMileage is not null && request.MaxMileage < 0)
        {
            throw new InvalidSearchFilterException("maxMileage must be >= 0");
        }

        var cars = await carRepository.SearchAsync(request, cancellationToken);

        return new CarListResponse(cars.Select(CarMapper.ToResponse));
    }
}
