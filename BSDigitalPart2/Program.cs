using Shared.Interfaces;
using Shared.Services;

namespace BSDigitalPart2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddTransient<IPriceEvaluationService, PriceEvaluationService>();

            // Add services to the container.

            builder.Services.AddControllers();

            builder.WebHost.UseKestrel(options =>
            {
                options.ListenAnyIP(5000); // Listen on port 5000 for HTTP
                options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // Listen on port 5001 for HTTPS
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
