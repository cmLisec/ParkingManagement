using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ParkingManagement;
using ParkingManagement.Domain.Repositories.v1;
using ParkingManagement.Domain.Services.v1;

internal class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
