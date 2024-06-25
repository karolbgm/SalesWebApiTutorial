using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebApiTutorial.Data;
namespace SalesWebApiTutorial;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContex") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
        //?? : if it's null, throw this error

        builder.Services.AddCors(); //security feature that limits what can access 


        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); 
        //AllowAnyOrigin - any other IP address can access our server
        //AllowAnyHeader - Any header in the request - you don't need a token
        
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
