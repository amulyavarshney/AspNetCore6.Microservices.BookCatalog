using BookQuery.Service.Context;
using BookQuery.Service.Models;
using BookQuery.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BookQuery.Service
{
    /* The above class is the entry point of the application. It creates a web application builder,
    adds the database context, adds the controllers, adds the hosted service, adds the
    configuration, adds the services, adds the swagger, builds the application, and runs the
    application */
    public class Program
    {
        /// <summary>
        /// The above function is used to create a web application builder, add a database context, add
        /// controllers, add a hosted service, configure RabbitMQ, add scoped and singleton services,
        /// add Swagger, and build the application
        /// </summary>
        /// <param name="args">The arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            /* Creating a web application builder. */
            var builder = WebApplication.CreateBuilder(args);

            /* Adding the database context to the application. */
            builder.Services.AddDbContext<BookContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
            });

            /* Adding the controllers to the application. */
            builder.Services.AddControllers();

            /* Adding the hosted service to the application. */
            builder.Services.AddHostedService<RabbitMQConsumerService>();

            /* Configuring the RabbitMQConfig class with the values from the RabbitMQConfig section of
            the appsettings.json file. */
            builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQConfig"));

            /* Adding the BookService class to the application as a scoped service. */
            builder.Services.AddScoped<IBookService, BookService>();

            /* Adding the BookUpdateService and BookUpdateContext to the application as a singleton. */
            builder.Services.AddSingleton<IBookUpdateService, BookUpdateService>();
            builder.Services.AddSingleton<BookUpdateContext>();

            /* Adding the Swagger documentation to the application. */
            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1.0.0", new OpenApiInfo
                {
                    Title = "Book Query Service Documentation",
                });
            });

            /* Building the application. */
            var app = builder.Build();

            /* Checking if the environment is development. If it is, it is adding swagger to the
            application. */
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "Books Query System");
                });
            }

            /* Mapping the controllers and running the application. */
            app.MapControllers();
            app.Run();
        }
    }
}