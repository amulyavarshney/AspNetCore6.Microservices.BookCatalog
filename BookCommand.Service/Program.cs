using BookCommand.Service.Context;
using BookCommand.Service.Models;
using BookCommand.Service.MQ;
using BookCommand.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BookCommand.Service
{
    /* This is the main entry point of the application. It is the first code that is executed when the 
    application starts. */
    public class Program
    {
        /// <summary>
        /// The Main function is creating a new instance of the WebApplication class and registering the
        /// services that the application will use
        /// </summary>
        /// <param name="args">The arguments that are passed to the application.</param>
        public static void Main(string[] args)
        {
            /* Creating a new instance of the WebApplication class and registering the services that 
            the application will use. */
            var builder = WebApplication.CreateBuilder(args);

            /* Adding the BookContext to the application. */
            builder.Services.AddDbContext<BookContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Development"));
            });

            /* Adding the controllers to the application. */
            builder.Services.AddControllers();

            /* Adding the RabbitMQ service to the application. */
            builder.Services.AddRabbitMQ(builder.Configuration);

            /* Getting the RabbitMQConfig section from the appsettings.json file and then configuring
            the RabbitMQConfig class with the values from the appsettings.json file. */
            builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQConfig"));

            /* Registering the IBookService interface with the BookService class. */
            builder.Services.AddScoped<IBookService, BookService>();

            /* Adding Swagger to the application. */
            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1.0.0", new OpenApiInfo
                {
                    Title = "Book Command Service Documentation",
                });
            });

            /* Building the application. */
            var app = builder.Build();

            /* Checking if the environment is development. If it is, it is using Swagger and SwaggerUI. */
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "Books Command System");
                });
            }

            /* Mapping the controllers to the application. */
            app.MapControllers();

            /* Running the application. */
            app.Run();
        }
    }
}