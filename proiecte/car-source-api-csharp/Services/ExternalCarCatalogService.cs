using mock_car_source_api.Models;

namespace mock_car_source_api.Services;

public class ExternalCarCatalogService
{
    private const int DefaultCount = 120;

    private volatile IReadOnlyList<ExternalCarDto> _cars;

    public ExternalCarCatalogService()
    {
        _cars = GenerateCars(DefaultCount);
    }

    private static readonly Dictionary<string, string[]> BrandModels = new()
    {
        ["BMW"] = ["X1", "X3", "X5", "320i", "520d"],
        ["Audi"] = ["A3", "A4", "A6", "Q3", "Q5"],
        ["Mercedes-Benz"] = ["A-Class", "C-Class", "E-Class", "GLA", "GLE"],
        ["Volkswagen"] = ["Golf", "Passat", "Tiguan", "Arteon", "Touareg"],
        ["Toyota"] = ["Corolla", "Camry", "RAV4", "C-HR", "Yaris"],
        ["Ford"] = ["Focus", "Mondeo", "Kuga", "Puma", "Mustang"],
        ["Skoda"] = ["Fabia", "Octavia", "Superb", "Karoq", "Kodiaq"],
        ["Hyundai"] = ["i20", "i30", "Elantra", "Tucson", "Santa Fe"],
        ["Kia"] = ["Ceed", "Sportage", "Sorento", "Stonic", "Niro"],
        ["Peugeot"] = ["208", "308", "3008", "5008", "508"],
        ["Renault"] = ["Clio", "Megane", "Kadjar", "Austral", "Talisman"],
        ["Dacia"] = ["Logan", "Sandero", "Duster", "Jogger", "Spring"],
        ["Honda"] = ["Civic", "Accord", "CR-V", "HR-V", "Jazz"],
        ["Nissan"] = ["Micra", "Qashqai", "X-Trail", "Juke", "Leaf"],
        ["Mazda"] = ["Mazda2", "Mazda3", "Mazda6", "CX-3", "CX-5"],
        ["Volvo"] = ["S60", "S90", "XC40", "XC60", "XC90"],
        ["Tesla"] = ["Model 3", "Model S", "Model X", "Model Y"],
        ["Opel"] = ["Corsa", "Astra", "Insignia", "Mokka", "Grandland"],
        ["Seat"] = ["Ibiza", "Leon", "Ateca", "Arona", "Tarraco"],
        ["Cupra"] = ["Formentor", "Leon", "Born", "Ateca"],
        ["Alfa Romeo"] = ["Giulia", "Stelvio", "Tonale", "Giulietta"],
        ["Chevrolet"] = ["Spark", "Cruze", "Malibu", "Captiva", "Camaro"],
        ["Citroen"] = ["C3", "C4", "C5 X", "Berlingo", "Aircross"],
        ["Fiat"] = ["500", "Panda", "Tipo", "500X", "Doblo"],
        ["Jaguar"] = ["XE", "XF", "F-Pace", "E-Pace", "I-Pace"],
        ["Jeep"] = ["Renegade", "Compass", "Cherokee", "Wrangler", "Avenger"],
        ["Land Rover"] = ["Discovery", "Defender", "Range Rover Evoque", "Range Rover Sport"],
        ["Lexus"] = ["IS", "ES", "NX", "RX", "UX"],
        ["Mini"] = ["Cooper", "Countryman", "Clubman", "Paceman"],
        ["Mitsubishi"] = ["Space Star", "ASX", "Eclipse Cross", "Outlander", "L200"],
        ["Porsche"] = ["Macan", "Cayenne", "Panamera", "Taycan", "911"],
        ["Subaru"] = ["Impreza", "Legacy", "Forester", "Outback", "XV"],
        ["Suzuki"] = ["Swift", "Vitara", "S-Cross", "Ignis", "Jimny"]
    };

    private static readonly string[] FuelTypes =
    [
        "Petrol", "Diesel", "Hybrid", "Plug-in Hybrid", "Electric"
    ];

    private static readonly string[] Transmissions =
    [
        "Manual", "Automatic", "Automatic (DCT)", "Automatic (CVT)"
    ];

    private static readonly string[] Colors =
    [
        "Black", "White", "Gray", "Blue", "Red", "Silver", "Green", "Brown"
    ];

    public IReadOnlyList<ExternalCarDto> GetAll() => _cars;

    public ExternalCarDto? GetById(string externalId) => _cars.FirstOrDefault(c => c.ExternalId == externalId);

    public void Regenerate(int count) => _cars = GenerateCars(count);

    private static List<ExternalCarDto> GenerateCars(int count)
    {
        var cars = new List<ExternalCarDto>(count);

        for (var i = 0; i < count; i++)
        {
            cars.Add(GenerateCar(Guid.NewGuid().ToString()));
        }

        return cars;
    }

    private static ExternalCarDto GenerateCar(string externalId)
    {
        var brand = PickBrand();
        var model = Pick(BrandModels[brand]);

        return new ExternalCarDto
        {
            ExternalId = externalId,
            Brand = brand,
            Model = model,
            Year = Random.Shared.Next(2012, 2026),
            PriceEur = Random.Shared.Next(6000, 75001),
            MileageKm = Random.Shared.Next(10000, 220001),
            FuelType = Pick(FuelTypes),
            Transmission = Pick(Transmissions),
            Color = Pick(Colors),
            PublishedAtUtc = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 90))
        };
    }

    private static string Pick(string[] values)
    {
        return values[Random.Shared.Next(values.Length)];
    }

    private static string PickBrand()
    {
        return BrandModels.Keys.ElementAt(Random.Shared.Next(BrandModels.Count));
    }
}
