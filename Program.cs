using e_commerce_basic.Common;
using e_commerce_basic.Database;
using e_commerce_basic.Database.Seedings;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Middlewares;
using e_commerce_basic.Models;
using e_commerce_basic.Services;
using e_commerce_basic.Services.Auth;
using e_commerce_basic.Services.Email;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//config swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

// config Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDBContext>()
.AddDefaultTokenProviders();

// Config JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var signingKey = builder.Configuration["JWT:SigningKey"]
    ?? throw new InvalidOperationException("JWT:SigningKey is missing");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(signingKey)
        ),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Config firebase
var firebaseOptions = builder.Configuration
    .GetSection("Firebase")
    .Get<Firebase>()
    ?? throw new Exception("Firebase service account config missing");

// Tạo initializer cho Service Account
var initializer = new ServiceAccountCredential.Initializer(
        firebaseOptions.ClientEmail)
{
    ProjectId = firebaseOptions.ProjectId,
    KeyId = firebaseOptions.PrivateKeyId,
    Scopes = new[]
    {
        "https://www.googleapis.com/auth/cloud-platform"
    }
};

var privateKey = firebaseOptions.PrivateKey
    .Replace("\\n", "\n")
    .Trim();

// Gắn private key
var serviceAccountCredential = new ServiceAccountCredential(
    initializer.FromPrivateKey(privateKey)
);

// Convert sang GoogleCredential (KHÔNG obsolete)
GoogleCredential credential = serviceAccountCredential.ToGoogleCredential();

// Init Firebase
FirebaseApp.Create(new AppOptions
{
    Credential = credential,
    ProjectId = firebaseOptions.ProjectId
});

// config controller
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<EmailConfirmationService>();
builder.Services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

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

// config auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
