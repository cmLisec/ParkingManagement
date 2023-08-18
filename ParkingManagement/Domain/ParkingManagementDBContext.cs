
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ParkingManagement.Domain.Models;
using System;

namespace ParkingManagement
{
    /// <summary>
    /// ParkingManagementDBContext 
    /// </summary>
    public class ParkingManagementDBContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public ParkingManagementDBContext(DbContextOptions<ParkingManagementDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Server = dev-shop-01-sql.database.windows.net; Database = TestCm; User Id = LisecAdmin; Password = evenly-2PNPSKFX7;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection);
            }
        }
    }
}
