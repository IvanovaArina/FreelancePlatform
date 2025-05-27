//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using FreelancePlatform.Infrastructure.Data;
//using FreelancePlatform.Infrastructure.Repository;
//using FreelancePlatform.Application.Services;
//using FreelancePlatform.Application.DesignPatterns.Command;
//using FreelancePlatform.Application.DesignPatterns.Facade;
//using FreelancePlatform.Application.DesignPatterns.Observer;
//using AutoMapper;
//using FreelancePlatform.Application.Mappers;
//using FreelancePlatform.Application.DesignPatterns.Adapter;
//using FreelancePlatform.Application.DesignPatterns.Strategy;
//using FreelancePlatform.Domain.Interfaces;
//using FreelancePlatform.Infrastructure.Repository;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Configure PostgreSQL
//builder.Services.AddDbContext<FreelanceDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
//        b => b.MigrationsAssembly("FreelancePlatform.Infrastructure")));
//// Configure AutoMapper
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//// Register repositories
//builder.Services.AddScoped<IOrderRepository, PGOrderRepository>();
//builder.Services.AddScoped<IUserRepository, PGUserRepository>();
//builder.Services.AddScoped<IProfileRepository, PGProfileRepository>();
//builder.Services.AddScoped<ITransactionRepository, PGTransactionRepository>();
//builder.Services.AddScoped<IUserCredentialsRepository, PGUserCredentialRepository>();

//// Register services
//builder.Services.AddScoped<OrderService>();
//builder.Services.AddScoped<UserService>();
//builder.Services.AddScoped<ProfileService>();
//builder.Services.AddScoped<PaymentService>();
//builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<OrderFacade>();
//builder.Services.AddSingleton<NotificationService>();
//builder.Services.AddSingleton<CommandInvoker>();

//// Register payment processor and payment context
//builder.Services.AddSingleton<IPaymentProcessor, StripeAdapter>(); // По умолчанию используем Stripe
//builder.Services.AddSingleton<OrderPaymentContext>(sp =>
//{
//    var context = new OrderPaymentContext();
//    context.SetStrategy(new FixedPriceStrategy()); // По умолчанию фиксированная цена
//    return context;
//});

//// Configure JWT Authentication
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//app.UseSwagger();
//app.UseSwaggerUI();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();

// FreelancePlatform.Presentation/Program.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FreelancePlatform.Infrastructure.Data;
using FreelancePlatform.Infrastructure.Repository;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DesignPatterns.Command;
using FreelancePlatform.Application.DesignPatterns.Facade;
using FreelancePlatform.Application.DesignPatterns.Observer;
using AutoMapper;
using FreelancePlatform.Application.Mappers;
using FreelancePlatform.Application.DesignPatterns.Adapter;
using FreelancePlatform.Application.DesignPatterns.Strategy;
using FreelancePlatform.Domain.Interfaces;
using Microsoft.OpenApi.Models;
using FreelancePlatform.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.ModelValidatorProviders.Clear();
        // Убрали options.Filters.Add(new Microsoft.AspNetCore.Mvc.ValidateModelStateFilter());
    });
builder.Services.AddEndpointsApiExplorer();

// Настройка Swagger с поддержкой JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreelancePlatform API", Version = "v1" });

    // Добавляем схему безопасности для JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure PostgreSQL with migrations assembly
builder.Services.AddDbContext<FreelanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("FreelancePlatform.Infrastructure")));

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Register repositories
builder.Services.AddScoped<IOrderRepository, PGOrderRepository>();
builder.Services.AddScoped<IUserRepository, PGUserRepository>();
builder.Services.AddScoped<IProfileRepository, PGProfileRepository>();
builder.Services.AddScoped<ITransactionRepository, PGTransactionRepository>();
builder.Services.AddScoped<IUserCredentialsRepository, PGUserCredentialRepository>();

// Register services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<OrderFacade>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddSingleton<CommandInvoker>();

// Register payment processor and payment context
builder.Services.AddSingleton<IPaymentProcessor, StripeAdapter>();
builder.Services.AddSingleton<OrderPaymentContext>(sp =>
{
    var context = new OrderPaymentContext();
    context.SetStrategy(new FixedPriceStrategy());
    return context;
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];
        var jwtAudience = builder.Configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        {
            throw new ArgumentNullException("JWT configuration (Key, Issuer, or Audience) is missing in appsettings.json");
        }

        var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
        if (keyBytes.Length < 32)
        {
            throw new ArgumentException($"JWT Key must be at least 256 bits (32 bytes), but was {keyBytes.Length} bytes.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreelancePlatform API V1");
    c.RoutePrefix = "swagger";
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();