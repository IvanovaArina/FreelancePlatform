using MassTransit;
using Minio;

namespace ImageService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS
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

            // ✅ MinIO — ВАЖНО
            builder.Services.AddMinio(config =>
            {
                config
                    .WithEndpoint("minio:9000") 
                    .WithCredentials("minioadmin", "minioadmin123")
                    .WithSSL(false);
            });

            // MassTransit + RabbitMQ
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<ProjectImageUploadedConsumer>();
                x.AddConsumer<ProjectDeletedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ReceiveEndpoint("image-service-queue", e =>
                    {
                        e.ConfigureConsumer<ProjectImageUploadedConsumer>(context);
                        e.ConfigureConsumer<ProjectDeletedConsumer>(context);
                    });
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll"); 
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
