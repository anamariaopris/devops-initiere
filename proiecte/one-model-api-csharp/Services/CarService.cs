using Microsoft.EntityFrameworkCore;
using OneModelApi.Data;
using OneModelApi.Dtos;
using OneModelApi.Errors;
using OneModelApi.Models;

namespace OneModelApi.Services;

public class CarService(AppDbContext db)
{
    public Task<CarListResponse> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Cars, cancellationToken);

    public Task<CarListResponse> GetByBrandAsync(string brand, CancellationToken cancellationToken) =>
        ListAsync(db.Cars.Where(c => c.Brand == brand), cancellationToken);

    public Task<CarListResponse> GetByColorAsync(string color, CancellationToken cancellationToken) =>
        ListAsync(db.Cars.Where(c => c.Color == color), cancellationToken);

    public Task<CarListResponse> GetByFuelTypeAsync(string fuelType, CancellationToken cancellationToken) =>
        ListAsync(db.Cars.Where(c => c.FuelType == fuelType), cancellationToken);

    public Task<CarListResponse> GetByTransmissionAsync(string transmission, CancellationToken cancellationToken) =>
        ListAsync(db.Cars.Where(c => c.Transmission == transmission), cancellationToken);

    public Task<CarListResponse> GetByAvailableAsync(bool available, CancellationToken cancellationToken) =>
        ListAsync(db.Cars.Where(c => c.Available == available), cancellationToken);

    public async Task<CarResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var car = await db.Cars.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return car.ToResponse();
    }

    public Task<CarListResponse> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.MinYear > request.MaxYear)
        {
            throw new BadRequestException($"Invalid year range: minYear ({request.MinYear}) must be <= maxYear ({request.MaxYear})");
        }
        if (request.MaxPrice < 0)
        {
            throw new BadRequestException("maxPrice must be >= 0");
        }
        if (request.MaxMileage < 0)
        {
            throw new BadRequestException("maxMileage must be >= 0");
        }

        var query = db.Cars.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Brand)) query = query.Where(c => c.Brand == request.Brand);
        if (!string.IsNullOrWhiteSpace(request.Model)) query = query.Where(c => c.Model == request.Model);
        if (!string.IsNullOrWhiteSpace(request.Color)) query = query.Where(c => c.Color == request.Color);
        if (request.MinYear is not null) query = query.Where(c => c.Year >= request.MinYear);
        if (request.MaxYear is not null) query = query.Where(c => c.Year <= request.MaxYear);
        if (request.MaxPrice is not null) query = query.Where(c => c.Price <= request.MaxPrice);
        if (request.MaxMileage is not null) query = query.Where(c => c.Mileage <= request.MaxMileage);
        if (!string.IsNullOrWhiteSpace(request.FuelType)) query = query.Where(c => c.FuelType == request.FuelType);
        if (!string.IsNullOrWhiteSpace(request.Transmission)) query = query.Where(c => c.Transmission == request.Transmission);
        if (request.Available is not null) query = query.Where(c => c.Available == request.Available);
        return ListAsync(query, cancellationToken);
    }

    public async Task<CarResponse> CreateAsync(CarRequest request, CancellationToken cancellationToken)
    {
        await EnsureUniqueAsync(request.Brand, request.Model, null, cancellationToken);
        var car = new Car();
        Copy(request, car);
        db.Cars.Add(car);
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    public async Task<CarResponse> UpdateAsync(long id, CarRequest request, CancellationToken cancellationToken)
    {
        var car = await FindAsync(id, cancellationToken);
        await EnsureUniqueAsync(request.Brand, request.Model, id, cancellationToken);
        Copy(request, car);
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    public async Task<CarResponse> PatchAsync(long id, CarPatchRequest request, CancellationToken cancellationToken)
    {
        var car = await FindAsync(id, cancellationToken);
        if (IsEmpty(request))
        {
            throw new BadRequestException("Patch request must contain at least one field");
        }

        car.Brand = request.Brand ?? car.Brand;
        car.Model = request.Model ?? car.Model;
        car.Color = request.Color ?? car.Color;
        car.Size = request.Size ?? car.Size;
        car.Year = request.Year ?? car.Year;
        car.Price = request.Price ?? car.Price;
        car.Mileage = request.Mileage ?? car.Mileage;
        car.FuelType = request.FuelType ?? car.FuelType;
        car.Transmission = request.Transmission ?? car.Transmission;
        car.Available = request.Available ?? car.Available;

        await EnsureUniqueAsync(car.Brand, car.Model, id, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var car = await FindAsync(id, cancellationToken);
        db.Cars.Remove(car);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static async Task<CarListResponse> ListAsync(IQueryable<Car> query, CancellationToken cancellationToken)
    {
        var cars = await query.AsNoTracking().ToListAsync(cancellationToken);
        return new CarListResponse(cars.Select(c => c.ToResponse()));
    }

    private async Task<Car> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Cars.FirstOrDefaultAsync(c => c.Id == id, cancellationToken) ?? throw NotFound(id);

    private async Task EnsureUniqueAsync(string brand, string model, long? excludedId, CancellationToken cancellationToken)
    {
        if (await db.Cars.AnyAsync(c => c.Brand == brand && c.Model == model && c.Id != excludedId, cancellationToken))
        {
            throw new ConflictException($"Car already exists: {brand} {model}");
        }
    }

    private static bool IsEmpty(CarPatchRequest request) => request is
    {
        Brand: null, Model: null, Color: null, Size: null, Year: null,
        Price: null, Mileage: null, FuelType: null, Transmission: null, Available: null
    };

    private static NotFoundException NotFound(long id) => new($"Car not found with id {id}");

    private static void Copy(CarRequest request, Car car)
    {
        car.Brand = request.Brand;
        car.Model = request.Model;
        car.Color = request.Color;
        car.Size = request.Size;
        car.Year = request.Year;
        car.Price = request.Price;
        car.Mileage = request.Mileage;
        car.FuelType = request.FuelType;
        car.Transmission = request.Transmission;
        car.Available = request.Available;
    }
}
