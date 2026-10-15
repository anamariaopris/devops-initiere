namespace OneToManyApi.Dtos;

public class CarSearchRequest
{
    public string? Brand { get; init; }
    public string? Model { get; init; }
    public long? UserId { get; init; }
    public int? MinYear { get; init; }
    public int? MaxYear { get; init; }
}
