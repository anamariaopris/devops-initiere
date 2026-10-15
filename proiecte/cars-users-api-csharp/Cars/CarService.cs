using Microsoft.EntityFrameworkCore;
using OneToManyApi.Data;
using OneToManyApi.Errors;

namespace OneToManyApi.Cars;

public class CarService(AppDbContext db)
{
    public Task<CarListResponse> GetAllAsync(CancellationToken cancellationToken) =>
        ListAsync(db.Cars, cancellationToken);

    public async Task<CarResponse> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var car = await db.Cars.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw NotFound(id);
        return car.ToResponse();
    }

    public async Task<UserCarsResponse> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(userId, cancellationToken);
        var cars = await ListAsync(db.Cars.Where(c => c.UserId == userId), cancellationToken);
        return new UserCarsResponse(userId, cars.Cars);
    }

    public Task<CarListResponse> GetByBrandAsync(string brand, CancellationToken cancellationToken) =>
        ListAsync(db.Cars.Where(c => c.Brand == brand), cancellationToken);

    public Task<CarListResponse> GetByYearRangeAsync(int minYear, int maxYear, CancellationToken cancellationToken)
    {
        EnsureValidYearRange(minYear, maxYear);
        return ListAsync(db.Cars.Where(c => c.Year >= minYear && c.Year <= maxYear), cancellationToken);
    }

    public Task<CarListResponse> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken)
    {
        if (request.MinYear is not null && request.MaxYear is not null)
        {
            EnsureValidYearRange(request.MinYear.Value, request.MaxYear.Value);
        }

        var query = db.Cars.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Brand)) query = query.Where(c => c.Brand == request.Brand);
        if (!string.IsNullOrWhiteSpace(request.Model)) query = query.Where(c => c.Model == request.Model);
        if (request.UserId is not null) query = query.Where(c => c.UserId == request.UserId);
        if (request.MinYear is not null) query = query.Where(c => c.Year >= request.MinYear);
        if (request.MaxYear is not null) query = query.Where(c => c.Year <= request.MaxYear);
        return ListAsync(query, cancellationToken);
    }

    public async Task<CarResponse> CreateAsync(CarRequest request, CancellationToken cancellationToken)
    {
        await EnsureUserExistsAsync(request.UserId, cancellationToken);
        var car = new Car { Brand = request.Brand, Model = request.Model, Year = request.Year, UserId = request.UserId };
        db.Cars.Add(car);
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    public async Task<CarResponse> UpdateAsync(long id, CarRequest request, CancellationToken cancellationToken)
    {
        var car = await FindAsync(id, cancellationToken);
        await EnsureUserExistsAsync(request.UserId, cancellationToken);
        car.Brand = request.Brand;
        car.Model = request.Model;
        car.Year = request.Year;
        car.UserId = request.UserId;
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    public async Task<CarResponse> PatchAsync(long id, CarPatchRequest request, CancellationToken cancellationToken)
    {
        var car = await FindAsync(id, cancellationToken);
        if (request is { Brand: null, Model: null, Year: null, UserId: null })
        {
            throw new BadRequestException("Patch request must contain at least one field");
        }

        if (request.UserId is not null)
        {
            await EnsureUserExistsAsync(request.UserId.Value, cancellationToken);
        }

        car.Brand = request.Brand ?? car.Brand;
        car.Model = request.Model ?? car.Model;
        car.Year = request.Year ?? car.Year;
        car.UserId = request.UserId ?? car.UserId;
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    public async Task<CarResponse> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var car = await FindAsync(id, cancellationToken);
        db.Cars.Remove(car);
        await db.SaveChangesAsync(cancellationToken);
        return car.ToResponse();
    }

    private static async Task<CarListResponse> ListAsync(IQueryable<Car> query, CancellationToken cancellationToken)
    {
        var cars = await query.AsNoTracking().OrderBy(c => c.Id).ToListAsync(cancellationToken);
        return new CarListResponse(cars.Select(c => c.ToResponse()));
    }

    private async Task<Car> FindAsync(long id, CancellationToken cancellationToken) =>
        await db.Cars.FirstOrDefaultAsync(c => c.Id == id, cancellationToken) ?? throw NotFound(id);

    private async Task EnsureUserExistsAsync(long userId, CancellationToken cancellationToken)
    {
        if (!await db.Users.AnyAsync(u => u.Id == userId, cancellationToken))
        {
            throw new NotFoundException($"User not found with id {userId}");
        }
    }

    private static void EnsureValidYearRange(int minYear, int maxYear)
    {
        if (minYear > maxYear)
        {
            throw new BadRequestException($"Invalid year range: minYear ({minYear}) must be <= maxYear ({maxYear})");
        }
    }

    private static NotFoundException NotFound(long id) => new($"Car not found with id {id}");
}
