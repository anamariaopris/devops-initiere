using OneToManyApi.Models;

namespace OneToManyApi.Data.Seed;

public static class InitialData
{
    public static readonly User[] Users =
    [
        new User { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
        new User { Id = 2, Name = "Jane Smith", Email = "jane.smith@example.com" }
    ];

    public static readonly Car[] Cars =
    [
        new Car { Id = 1, Brand = "Toyota", Model = "Corolla", Year = 2020, UserId = 1 },
        new Car { Id = 2, Brand = "Honda", Model = "Civic", Year = 2021, UserId = 1 },
        new Car { Id = 3, Brand = "Ford", Model = "Focus", Year = 2019, UserId = 2 }
    ];
}
