using Microsoft.EntityFrameworkCore;
using ParkingManagement;
using ParkingManagement.Domain.Repositories.v1;
using ParkingManagement.Domain.Services.v1;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var optionsBuilder = new DbContextOptionsBuilder<ParkingManagementDBContext>();

        string connection = "Server=tcp:parknow.database.windows.net,1433;Initial Catalog=ParkNow;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
        optionsBuilder.UseSqlServer(connection);
        builder.Services.AddDbContext<ParkingManagementDBContext>(provider => new ParkingManagementDBContext(optionsBuilder.Options));
        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<UsersRepository>();
        builder.Services.AddScoped<ParkingCardService>();
        builder.Services.AddScoped<ParkingCardRepository>();
        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParkingManagement_v1"));
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}