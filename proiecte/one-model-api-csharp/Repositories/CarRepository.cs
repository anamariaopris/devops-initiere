using Microsoft.EntityFrameworkCore;
using OneModelApi.Data;
using OneModelApi.Dtos;
using OneModelApi.Models;

namespace OneModelApi.Repositories;

public class CarRepository(AppDbContext dbContext) : ICarRepository
{
    public Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().ToListAsync(cancellationToken);

    public Task<Car?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Car?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<List<Car>> GetByBrandAsync(string brand, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(c => c.Brand == brand).ToListAsync(cancellationToken);

    public Task<List<Car>> GetByColorAsync(string color, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(c => c.Color == color).ToListAsync(cancellationToken);

    public Task<List<Car>> GetByFuelTypeAsync(string fuelType, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(c => c.FuelType == fuelType).ToListAsync(cancellationToken);

    public Task<List<Car>> GetByTransmissionAsync(string transmission, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(c => c.Transmission == transmission).ToListAsync(cancellationToken);

    public Task<List<Car>> GetByAvailableAsync(bool available, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(c => c.Available == available).ToListAsync(cancellationToken);

    public async Task<List<Car>> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Cars.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Brand)) query = query.Where(c => c.Brand == request.Brand);
        if (!string.IsNullOrWhiteSpace(request.Model)) query = query.Where(c => c.Model == request.Model);
        if (!string.IsNullOrWhiteSpace(request.Color)) query = query.Where(c => c.Color == request.Color);
        if (request.MinYear is not null) query = query.Where(c => c.Year >= request.MinYear.Value);
        if (request.MaxYear is not null) query = query.Where(c => c.Year <= request.MaxYear.Value);
        if (request.MaxPrice is not null) query = query.Where(c => c.Price <= request.MaxPrice.Value);
        if (request.MaxMileage is not null) query = query.Where(c => c.Mileage <= request.MaxMileage.Value);
        if (!string.IsNullOrWhiteSpace(request.FuelType)) query = query.Where(c => c.FuelType == request.FuelType);
        if (!string.IsNullOrWhiteSpace(request.Transmission)) query = query.Where(c => c.Transmission == request.Transmission);
        if (request.Available is not null) query = query.Where(c => c.Available == request.Available.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByBrandModelAsync(
        string brand,
        string model,
        long? excludedId = null,
        CancellationToken cancellationToken = default)
        => excludedId is null
            ? dbContext.Cars.AnyAsync(c => c.Brand == brand && c.Model == model, cancellationToken)
            : dbContext.Cars.AnyAsync(c => c.Id != excludedId.Value && c.Brand == brand && c.Model == model, cancellationToken);

    public async Task AddAsync(Car car, CancellationToken cancellationToken = default)
    {
        await dbContext.Cars.AddAsync(car, cancellationToken);
    }

    public Task DeleteAsync(Car car, CancellationToken cancellationToken = default)
    {
        dbContext.Cars.Remove(car);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
