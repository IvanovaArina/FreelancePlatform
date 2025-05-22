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
using FreelancePlatform.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL
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
builder.Services.AddSingleton<IPaymentProcessor, StripeAdapter>(); // По умолчанию используем Stripe
builder.Services.AddSingleton<OrderPaymentContext>(sp =>
{
    var context = new OrderPaymentContext();
    context.SetStrategy(new FixedPriceStrategy()); // По умолчанию фиксированная цена
    return context;
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();