using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Repositories.v1;
using ParkingManagement.Domain.Services.v1;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

namespace ParkingManagement
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private const string _openApiDescription = @"A delivery tracking open api service";
        /// <summary>
        /// Constructor of Startup.
        /// </summary>
        /// <param name="env">Specify IWebHostEnvironment.</param>
        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ParkingManagementDBContext>();

            string connection = "Server=tcp:parknow.database.windows.net,1433;Initial Catalog=ParkNow;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
            optionsBuilder.UseSqlServer(connection);
            services.AddDbContext<ParkingManagementDBContext>(provider => new ParkingManagementDBContext(optionsBuilder.Options));
            services.AddScoped<UsersService>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<ParkingCardService>();
            services.AddScoped<ParkingCardRepository>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking Management API", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking Management API V1");
            });
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                var controllerBuilder = endpoints.MapControllers();
                if (!env.IsDevelopment())
                    controllerBuilder.RequireAuthorization();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(@"Hello World!");
                });
            });
        }
    }
}
