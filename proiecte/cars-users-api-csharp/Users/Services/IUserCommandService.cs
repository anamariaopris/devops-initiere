using OneToManyApi.Dtos;

namespace OneToManyApi.Services;

public interface IUserCommandService
{
    Task<UserResponse> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> UpdateAsync(long id, UserUpdateRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> PatchAsync(long id, UserPatchRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> DeleteAsync(long id, CancellationToken cancellationToken = default);
}
