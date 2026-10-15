namespace OneToManyApi.Dtos;

public record UserWithCarsResponse(long Id, string Name, string Email, IEnumerable<CarResponse> Cars);
