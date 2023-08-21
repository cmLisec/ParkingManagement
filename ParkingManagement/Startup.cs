﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Repositories.v1;
using ParkingManagement.Domain.Services.v1;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;

namespace ParkingManagement
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
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

            string connection = "Data Source=parknow.database.windows.net;Initial Catalog=ParkNow;User ID=parknowAdmin;Password=parkNow@123;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            services.AddDbContext<ParkingManagementDBContext>(options =>
                                                              options.UseSqlServer(connection));
            services.AddScoped<UsersService>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<ParkingCardService>();
            services.AddScoped<ParkingCardRepository>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking Management API", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    scope.ServiceProvider.GetService<ParkingManagementDBContext>().Database.Migrate();
                }
            }
            catch(Exception ex)
            { }
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
