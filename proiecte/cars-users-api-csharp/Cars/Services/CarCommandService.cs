using AutoMapper;
using OneToManyApi.Dtos;
using OneToManyApi.Exceptions;
using OneToManyApi.Models;
using OneToManyApi.Repositories;

namespace OneToManyApi.Services;

public class CarCommandService(ICarRepository carRepository, IUserRepository userRepository, IMapper mapper) : ICarCommandService
{
    public async Task<CarResponse> CreateAsync(CarCreateRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new NotFoundException($"User not found with id {request.UserId}");
        }

        var car = mapper.Map<Car>(request);

        await carRepository.AddAsync(car, cancellationToken);
        await carRepository.SaveChangesAsync(cancellationToken);

        return mapper.Map<CarResponse>(car);
    }

    public async Task<CarResponse> UpdateAsync(long id, CarUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetTrackedByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Car not found with id {id}");

        if (car.UserId != request.UserId)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException($"User not found with id {request.UserId}");
            }
        }

        mapper.Map(request, car);

        await carRepository.SaveChangesAsync(cancellationToken);
        return mapper.Map<CarResponse>(car);
    }

    public async Task<CarResponse> PatchAsync(long id, CarPatchRequest request, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetTrackedByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Car not found with id {id}");

        if (request.UserId is not null && car.UserId != request.UserId.Value)
        {
            var user = await userRepository.GetByIdAsync(request.UserId.Value, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException($"User not found with id {request.UserId.Value}");
            }
        }

        mapper.Map(request, car);

        await carRepository.SaveChangesAsync(cancellationToken);
        return mapper.Map<CarResponse>(car);
    }

    public async Task<CarResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var car = await carRepository.GetTrackedByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Car not found with id {id}");

        var response = mapper.Map<CarResponse>(car);
        carRepository.Remove(car);
        await carRepository.SaveChangesAsync(cancellationToken);
        return response;
    }
}
