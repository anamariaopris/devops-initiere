using Microsoft.EntityFrameworkCore;
using OnlineShopApi.Models;
using OnlineShopApi.Security;
using OnlineShopApi.Services;

namespace OnlineShopApi.Data.Seed;

public class DataSeeder(AppDbContext dbContext, ILogger<DataSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedPermissionsAsync(cancellationToken);
        await SeedAdminsAsync(cancellationToken);
        await SeedCustomersAsync(cancellationToken);
        await SeedCatalogAsync(cancellationToken);
    }

    private async Task SeedPermissionsAsync(CancellationToken cancellationToken)
    {
        var existing = await dbContext.Permissions.Select(p => p.Name).ToListAsync(cancellationToken);
        foreach (var name in Permissions.All.Except(existing))
        {
            dbContext.Permissions.Add(new Permission { Name = name });
            logger.LogInformation("Seeded permission: {Name}", name);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedAdminsAsync(CancellationToken cancellationToken)
    {
        foreach (var seed in SeedCatalog.Admins)
        {
            if (await dbContext.Users.AnyAsync(u => u.Email == seed.Email, cancellationToken))
            {
                continue;
            }

            dbContext.Admins.Add(new Admin
            {
                User = await NewUserAsync(seed.Email, seed.Password, Permissions.All, cancellationToken),
                DisplayName = seed.DisplayName,
                Department = seed.Department,
                Notes = seed.Notes
            });
            logger.LogInformation("Seeded admin account: {Email}", seed.Email);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedCustomersAsync(CancellationToken cancellationToken)
    {
        foreach (var seed in SeedCatalog.Customers)
        {
            if (await dbContext.Users.AnyAsync(u => u.Email == seed.Email, cancellationToken))
            {
                continue;
            }

            dbContext.Customers.Add(new Customer
            {
                User = await NewUserAsync(seed.Email, seed.Password, Permissions.CustomerDefaults, cancellationToken),
                FullName = seed.FullName,
                BillingAddress = seed.BillingAddress,
                DefaultShippingAddress = seed.DefaultShippingAddress,
                Country = seed.Country,
                Phone = seed.Phone
            });
            logger.LogInformation("Seeded customer account: {Email}", seed.Email);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedCatalogAsync(CancellationToken cancellationToken)
    {
        if (await dbContext.Products.AnyAsync(cancellationToken))
        {
            return;
        }

        var products = SeedCatalog.Products.ToDictionary(p => p.Sku, p => new Product
        {
            Sku = p.Sku,
            Name = p.Name,
            Price = p.Price,
            Weight = p.Weight,
            Descriptions = p.Descriptions,
            Category = p.Category,
            CreateDate = p.CreateDate,
            Stock = p.Stock
        });
        dbContext.Products.AddRange(products.Values);

        var emails = SeedCatalog.Orders.Select(o => o.CustomerEmail).Distinct().ToList();
        var customers = await dbContext.Customers
            .Include(c => c.User)
            .Where(c => emails.Contains(c.User.Email))
            .ToDictionaryAsync(c => c.User.Email, cancellationToken);

        foreach (var seed in SeedCatalog.Orders.Where(o => customers.ContainsKey(o.CustomerEmail)))
        {
            var order = new Order
            {
                Customer = customers[seed.CustomerEmail],
                Ammount = seed.Ammount,
                ShippingAdress = seed.ShippingAdress,
                OrderAdress = seed.OrderAdress,
                OrderEmail = seed.CustomerEmail,
                OrderDate = seed.OrderDate,
                OrderStatus = seed.OrderStatus
            };

            foreach (var line in seed.Lines)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    Sku = line.Sku,
                    Price = line.Price,
                    Quantity = line.Quantity,
                    Product = products[line.Sku]
                });
            }

            dbContext.Orders.Add(order);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seeded {Products} products and {Orders} orders.", products.Count, SeedCatalog.Orders.Length);
    }

    private async Task<User> NewUserAsync(string email, string password, IEnumerable<string> permissionNames, CancellationToken cancellationToken)
    {
        var names = permissionNames.ToList();
        var permissions = await dbContext.Permissions.Where(p => names.Contains(p.Name)).ToListAsync(cancellationToken);
        return new User
        {
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password, AuthService.BCryptWorkFactor),
            Enabled = true,
            Permissions = permissions
        };
    }
}
