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

        string connection = "Server = parknow.cukvlrirhzja.eu-north-1.rds.amazonaws.com,1433; Database = parkNow; User Id = Admin; Password = test123456;Encrypt=true;TrustServerCertificate=true;";
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
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}