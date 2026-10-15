namespace OneToManyApi.Dtos;

public record UserCarsResponse(long UserId, IEnumerable<CarResponse> Cars);
