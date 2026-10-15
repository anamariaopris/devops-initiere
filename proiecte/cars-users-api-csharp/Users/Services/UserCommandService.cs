using AutoMapper;
using OneToManyApi.Dtos;
using OneToManyApi.Exceptions;
using OneToManyApi.Models;
using OneToManyApi.Repositories;

namespace OneToManyApi.Services;

public class UserCommandService(IUserRepository userRepository, IMapper mapper) : IUserCommandService
{
    public async Task<UserResponse> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new ConflictException($"User email already exists: {request.Email}");
        }

        var user = mapper.Map<User>(request);

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> UpdateAsync(long id, UserUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetTrackedByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User not found with id {id}");

        if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase) &&
            await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new ConflictException($"User email already exists: {request.Email}");
        }

        mapper.Map(request, user);

        await userRepository.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> PatchAsync(long id, UserPatchRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetTrackedByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User not found with id {id}");

        if (!string.IsNullOrWhiteSpace(request.Email) &&
            !string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase) &&
            await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new ConflictException($"User email already exists: {request.Email}");
        }

        mapper.Map(request, user);

        await userRepository.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetTrackedByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User not found with id {id}");

        var response = mapper.Map<UserResponse>(user);
        userRepository.Remove(user);
        await userRepository.SaveChangesAsync(cancellationToken);
        return response;
    }
}
