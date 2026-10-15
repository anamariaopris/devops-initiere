using OneToManyApi.Dtos;

namespace OneToManyApi.Services;

public interface IUserQueryService
{
    Task<UserWithCarsListResponse> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserWithCarsResponse> GetByIdWithCarsAsync(long id, CancellationToken cancellationToken = default);
    Task<UserResponse> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<UserResponse>> SearchAsync(UserSearchRequest request, CancellationToken cancellationToken = default);
}
