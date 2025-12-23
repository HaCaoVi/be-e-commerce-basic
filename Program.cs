using e_commerce_basic.Common;
using e_commerce_basic.Database;
using e_commerce_basic.Database.Seedings;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Middlewares;
using e_commerce_basic.Models;
using e_commerce_basic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

// Config database
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseMySql(
     builder.Configuration.GetConnectionString("DefaultConnection"),
     ServerVersion.AutoDetect(
         builder.Configuration.GetConnectionString("DefaultConnection")
     )
    );
});

// config validation error
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value!.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors.Select(e => e.ErrorMessage)
            );

        var response = ApiResponse<object>.Fail(
            "Validation failed",
            errors
        );

        return new BadRequestObjectResult(response);
    };
});

// config password
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDBContext>();

// config controller
builder.Services.AddControllers();

builder.Services.AddScoped<IAccountService, AccountService>();


var app = builder.Build();
// config role
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeed.SeedRolesAsync(services);
    await IdentitySeed.SeedAdminAsync(services);
}


// check connection
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    try
    {
        db.Database.CanConnect();
        Console.WriteLine("✅ Connected to MySQL successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Cannot connect to MySQL");
        Console.WriteLine(ex.Message);
    }
}

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// config exception
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

// config jwt
app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
