using OneModelApi.Dtos;
using OneModelApi.Exceptions;
using OneModelApi.Models;
using OneModelApi.Repositories;

namespace OneModelApi.Services;

public class CarCommandService(ICarRepository carRepository) : ICarCommandService
{
    public async Task<CarResponse> CreateAsync(CarCreateRequest request, CancellationToken cancellationToken = default)
    {
        if (await carRepository.ExistsByBrandModelAsync(request.Brand, request.Model, null, cancellationToken))
        {
            throw new CarAlreadyExistsException(request.Brand, request.Model);
        }

        var car = new Car
        {
            Brand = request.Brand,
            Model = request.Model,
            Color = request.Color,
            Size = request.Size,
            Year = request.Year,
            Price = request.Price,
            Mileage = request.Mileage,
            FuelType = request.FuelType,
            Transmission = request.Transmission,
            Available = request.Available
        };

        await carRepository.AddAsync(car, cancellationToken);
        await carRepository.SaveChangesAsync(cancellationToken);
        return CarMapper.ToResponse(car);
    }

    public async Task<CarResponse> UpdateAsync(long id, CarPutRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await carRepository.GetByIdForUpdateAsync(id, cancellationToken)
            ?? throw new CarNotFoundException(id);

        if (await carRepository.ExistsByBrandModelAsync(request.Brand, request.Model, id, cancellationToken))
        {
            throw new CarAlreadyExistsException(request.Brand, request.Model);
        }

        existing.Brand = request.Brand;
        existing.Model = request.Model;
        existing.Color = request.Color;
        existing.Size = request.Size;
        existing.Year = request.Year;
        existing.Price = request.Price;
        existing.Mileage = request.Mileage;
        existing.FuelType = request.FuelType;
        existing.Transmission = request.Transmission;
        existing.Available = request.Available;

        await carRepository.SaveChangesAsync(cancellationToken);
        return CarMapper.ToResponse(existing);
    }

    public async Task<CarResponse> PatchAsync(long id, CarPatchRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await carRepository.GetByIdForUpdateAsync(id, cancellationToken)
            ?? throw new CarNotFoundException(id);

        if (IsEmptyPatch(request))
        {
            throw new EmptyPatchRequestException();
        }

        if (request.Brand is not null) existing.Brand = request.Brand;
        if (request.Model is not null) existing.Model = request.Model;
        if (request.Color is not null) existing.Color = request.Color;
        if (request.Size is not null) existing.Size = request.Size.Value;
        if (request.Year is not null) existing.Year = request.Year.Value;
        if (request.Price is not null) existing.Price = request.Price.Value;
        if (request.Mileage is not null) existing.Mileage = request.Mileage.Value;
        if (request.FuelType is not null) existing.FuelType = request.FuelType;
        if (request.Transmission is not null) existing.Transmission = request.Transmission;
        if (request.Available is not null) existing.Available = request.Available.Value;

        if (await carRepository.ExistsByBrandModelAsync(existing.Brand, existing.Model, id, cancellationToken))
        {
            throw new CarAlreadyExistsException(existing.Brand, existing.Model);
        }

        await carRepository.SaveChangesAsync(cancellationToken);
        return CarMapper.ToResponse(existing);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var existing = await carRepository.GetByIdForUpdateAsync(id, cancellationToken)
            ?? throw new CarNotFoundException(id);

        await carRepository.DeleteAsync(existing, cancellationToken);
        await carRepository.SaveChangesAsync(cancellationToken);
    }

    private static bool IsEmptyPatch(CarPatchRequest request) =>
        request.Brand is null &&
        request.Model is null &&
        request.Color is null &&
        request.Size is null &&
        request.Year is null &&
        request.Price is null &&
        request.Mileage is null &&
        request.FuelType is null &&
        request.Transmission is null &&
        request.Available is null;
}
