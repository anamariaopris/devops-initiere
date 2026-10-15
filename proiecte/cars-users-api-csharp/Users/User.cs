using OneToManyApi.Cars;

namespace OneToManyApi.Users;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<Car> Cars { get; set; } = [];
}
