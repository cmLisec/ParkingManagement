using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ParkingManagement.Domain.Repositories.v1;
using ParkingManagement.Domain.Services.v1;
using System.Text;

namespace ParkingManagement
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration _configuration { get; }
        /// <summary>
        /// Constructor of Startup.
        /// </summary>
        /// <param name="env">Specify IWebHostEnvironment.</param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ParkingManagementDBContext>(options =>
                                                              options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<UsersService>();
            services.AddScoped<UsersRepository>();
            services.AddScoped<ParkingCardService>();
            services.AddScoped<ParkingCardRepository>();
            services.AddScoped<LoginService>();
            services.AddScoped<PaymentRepository>();
            services.AddScoped<PaymentService>();
            services.AddScoped<CardDetailsRepository>();
            services.AddScoped<CardDetailsService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = _configuration["Jwt:Issuer"],
                  ValidAudience = _configuration["Jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
              };
          });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parking Management API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
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
            catch (Exception ex)
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
            app.UseCors("AllowAll");

            app.UseRouting();
            app.UseAuthentication();
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
