using Microsoft.EntityFrameworkCore;
using ShopApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Missing connection string. Set the ConnectionStrings__Default environment variable.");
    return 1;
}

builder.Services.AddDbContext<ShopDb>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4))));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ShopDb>();
        db.Database.EnsureCreated();
        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Laptop", Price = 3499.99m, Stock = 5 },
                new Product { Name = "Mouse", Price = 79.90m, Stock = 40 },
                new Product { Name = "Monitor", Price = 899.00m, Stock = 12 });
            db.SaveChanges();
        }
    }
}
catch (Exception ex)
{
    app.Logger.LogCritical("Cannot reach the database: {Message}", ex.GetBaseException().Message);
    return 1;
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapHealthChecks("/health");

app.MapGet("/", () => new { app = "shop-api", host = Environment.MachineName });

app.MapGet("/products", async (ShopDb db) => await db.Products.ToListAsync());
app.MapGet("/products/{id:int}", async (int id, ShopDb db) =>
    await db.Products.FindAsync(id) is Product product ? Results.Ok(product) : Results.NotFound());
app.MapPost("/products", async (Product product, ShopDb db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});
app.MapPut("/products/{id:int}/stock", async (int id, int stock, ShopDb db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    product.Stock = stock;
    await db.SaveChangesAsync();
    return Results.Ok(product);
});

app.Run();

return 0;
