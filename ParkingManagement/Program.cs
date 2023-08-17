using Microsoft.EntityFrameworkCore;

using ParkingManagement;
using ParkingManagement.Domain.Repositories.v1;
using ParkingManagement.Domain.Services.v1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var optionsBuilder = new DbContextOptionsBuilder<ParkingManagementDBContext>();
optionsBuilder.UseSqlServer("");

builder.Services.AddDbContext<ParkingManagementDBContext>(provider => new ParkingManagementDBContext(optionsBuilder.Options));
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<UsersRepository>();


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
