using Microsoft.EntityFrameworkCore;
using OneToManyApi.Data;
using OneToManyApi.Dtos;
using OneToManyApi.Models;

namespace OneToManyApi.Repositories;

public class CarRepository(AppDbContext dbContext) : ICarRepository
{
    public Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().ToListAsync(cancellationToken);

    public Task<Car?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Car?> GetTrackedByIdAsync(long id, CancellationToken cancellationToken = default)
        => dbContext.Cars.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<Car>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(cancellationToken);

    public Task<List<Car>> GetByBrandAsync(string brand, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(x => x.Brand == brand).ToListAsync(cancellationToken);

    public Task<List<Car>> GetByYearRangeAsync(int minYear, int maxYear, CancellationToken cancellationToken = default)
        => dbContext.Cars.AsNoTracking().Where(x => x.Year >= minYear && x.Year <= maxYear).ToListAsync(cancellationToken);

    public Task<List<Car>> SearchAsync(CarSearchRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Cars.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Brand))
        {
            query = query.Where(x => x.Brand == request.Brand);
        }

        if (!string.IsNullOrWhiteSpace(request.Model))
        {
            query = query.Where(x => x.Model == request.Model);
        }

        if (request.UserId is not null)
        {
            query = query.Where(x => x.UserId == request.UserId.Value);
        }

        if (request.MinYear is not null)
        {
            query = query.Where(x => x.Year >= request.MinYear.Value);
        }

        if (request.MaxYear is not null)
        {
            query = query.Where(x => x.Year <= request.MaxYear.Value);
        }

        return query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Car car, CancellationToken cancellationToken = default)
    {
        await dbContext.Cars.AddAsync(car, cancellationToken);
    }

    public void Remove(Car car)
    {
        dbContext.Cars.Remove(car);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}
