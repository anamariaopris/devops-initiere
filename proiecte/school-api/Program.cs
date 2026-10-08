using Microsoft.EntityFrameworkCore;
using SchoolApi;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.Error.WriteLine("Missing connection string. Set the ConnectionStrings__Default environment variable.");
    return 1;
}

builder.Services.AddDbContext<SchoolDb>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 4))));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<SchoolDb>();
        db.Database.EnsureCreated();
        if (!db.Students.Any())
        {
            db.Students.AddRange(
                new Student { Name = "Maria Popescu", Email = "maria@school.ro" },
                new Student { Name = "Andrei Ionescu", Email = "andrei@school.ro" },
                new Student { Name = "Elena Dumitru", Email = "elena@school.ro" });
            db.Courses.AddRange(
                new Course { Title = "Linux", Credits = 5 },
                new Course { Title = "Docker", Credits = 6 });
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

app.MapGet("/", () => new { app = "school-api", host = Environment.MachineName });

app.MapGet("/students", async (SchoolDb db) => await db.Students.ToListAsync());
app.MapGet("/students/{id:int}", async (int id, SchoolDb db) =>
    await db.Students.FindAsync(id) is Student student ? Results.Ok(student) : Results.NotFound());
app.MapPost("/students", async (Student student, SchoolDb db) =>
{
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/students/{student.Id}", student);
});

app.MapGet("/courses", async (SchoolDb db) => await db.Courses.ToListAsync());
app.MapPost("/courses", async (Course course, SchoolDb db) =>
{
    db.Courses.Add(course);
    await db.SaveChangesAsync();
    return Results.Created($"/courses/{course.Id}", course);
});

app.Run();

return 0;
