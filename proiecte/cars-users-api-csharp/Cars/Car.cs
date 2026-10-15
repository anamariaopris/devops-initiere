using OneToManyApi.Users;

namespace OneToManyApi.Cars;

public class Car
{
    public long Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
}
