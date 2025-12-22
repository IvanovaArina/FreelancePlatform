using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using PortfolioService.Consumers;
using PortfolioService.Data;
using PortfolioService.Mappings;
using PortfolioService.Repoositories;
using PortfolioService.Repoositories.Interfaces;
using System.Text;

namespace PortfolioService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // jsonb support (если вдруг появится)
            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

            // -------------------------
            // DB
            // -------------------------
            builder.Services.AddDbContext<PortfolioDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("Default"),
                    npgsql => npgsql.EnableRetryOnFailure(3)
                )
            );

            // -------------------------
            // DI
            // -------------------------
            builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();

            // -------------------------
            // AutoMapper
            // -------------------------
            builder.Services.AddAutoMapper(typeof(PortfolioMappingProfile));

            // -------------------------
            // JWT
            // -------------------------
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
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        ),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            // -------------------------
            // MassTransit (RabbitMQ)
            // -------------------------
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<ImageProcessedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    // ⬇️ Docker hostname
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("portfolio-queue", e =>
                    {
                        e.ConfigureConsumer<ImageProcessedConsumer>(context);
                    });
                });
            });

            // -------------------------
            // Controllers + Swagger
            // -------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PortfolioService API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization. Example: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
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
                        Array.Empty<string>()
                    }
                });
            });

            // -------------------------
            // CORS
            // -------------------------
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
