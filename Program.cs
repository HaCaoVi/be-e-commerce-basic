using e_commerce_basic.Database;
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
var app = builder.Build();

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

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
