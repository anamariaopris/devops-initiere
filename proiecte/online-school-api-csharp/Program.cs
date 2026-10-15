using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OnlineSchoolApi.Users.Administrators.Repositories;
using OnlineSchoolApi.Users.Administrators.Services;
using OnlineSchoolApi.Auth.Services;
using OnlineSchoolApi.Books.Repositories;
using OnlineSchoolApi.Books.Services;
using OnlineSchoolApi.Common.Errors;
using OnlineSchoolApi.Common.Validation;
using OnlineSchoolApi.Courses.Repositories;
using OnlineSchoolApi.Courses.Services;
using OnlineSchoolApi.Data;
using OnlineSchoolApi.Data.Extensions;
using OnlineSchoolApi.Enrolments.Repositories;
using OnlineSchoolApi.Enrolments.Services;
using OnlineSchoolApi.Middleware;
using OnlineSchoolApi.Users.Permissions.Services;
using OnlineSchoolApi.Security;
using OnlineSchoolApi.Users.Students.Repositories;
using OnlineSchoolApi.Users.Students.Services;
using OnlineSchoolApi.Swagger;
using OnlineSchoolApi.Users.Teachers.Repositories;
using OnlineSchoolApi.Users.Teachers.Services;
using OnlineSchoolApi.Users.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options =>
    {
        options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(_ => ValidationMessages.MissingParameter);
        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => ValidationMessages.MissingParameter);
    })
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)))
    .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = ValidationErrorResponseFactory.Create);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiSwagger(builder.Configuration);
builder.Services.AddApiCors(builder.Configuration);
builder.Services.AddJwtSecurity(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IAdministratorRepository, AdministratorRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IEnrolmentRepository, EnrolmentRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IStudentCommandService, StudentCommandService>();
builder.Services.AddScoped<IStudentQueryService, StudentQueryService>();
builder.Services.AddScoped<ITeacherQueryService, TeacherQueryService>();
builder.Services.AddScoped<IAdministratorQueryService, AdministratorQueryService>();
builder.Services.AddScoped<ICourseCommandService, CourseCommandService>();
builder.Services.AddScoped<ICourseQueryService, CourseQueryService>();
builder.Services.AddScoped<IBookCommandService, BookCommandService>();
builder.Services.AddScoped<IBookQueryService, BookQueryService>();
builder.Services.AddScoped<IEnrolmentCommandService, EnrolmentCommandService>();
builder.Services.AddScoped<IEnrolmentQueryService, EnrolmentQueryService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors(CorsServiceExtensions.PolicyName);

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<PathAuthorizationMiddleware>();
app.MapControllers();

await app.ApplyDatabaseMigrationsAsync();

app.Run();
