using BookQuery.Service.Context;
using BookQuery.Service.Models;
using BookQuery.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BookQuery.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<BookContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
            });

            builder.Services.AddControllers();

            builder.Services.AddHostedService<RabbitMQConsumerService>();

            builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQConfig"));

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddSingleton<IBookUpdateService, BookUpdateService>();
            builder.Services.AddSingleton<BookUpdateContext>();

            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1.0.0", new OpenApiInfo
                {
                    Title = "Book Query Service Documentation",
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "Books Query System");
                });
            }

            app.MapControllers();
            app.Run();
        }
    }
}