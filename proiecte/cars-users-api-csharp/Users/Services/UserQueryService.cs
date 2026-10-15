using AutoMapper;
using OneToManyApi.Dtos;
using OneToManyApi.Exceptions;
using OneToManyApi.Repositories;

namespace OneToManyApi.Services;

public class UserQueryService(IUserRepository userRepository, IMapper mapper) : IUserQueryService
{
    public async Task<UserWithCarsListResponse> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        return mapper.Map<UserWithCarsListResponse>(users);
    }

    public async Task<UserWithCarsResponse> GetByIdWithCarsAsync(long id, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithCarsAsync(id, cancellationToken)
            ?? throw new NotFoundException($"User not found with id {id}");

        return mapper.Map<UserWithCarsResponse>(user);
    }

    public async Task<UserResponse> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new NotFoundException($"User not found with email {email}");

        return mapper.Map<UserResponse>(user);
    }

    public async Task<List<UserResponse>> SearchAsync(UserSearchRequest request, CancellationToken cancellationToken = default)
    {
        var users = await userRepository.SearchAsync(request, cancellationToken);
        return mapper.Map<List<UserResponse>>(users);
    }
}
