using Microsoft.EntityFrameworkCore;
using ParkingManagement.Domain.Models;

namespace ParkingManagement
{
    /// <summary>
    /// ParkingManagementDBContext 
    /// </summary>
    public class ParkingManagementDBContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<ParkingCard> ParkingCard { get; set; }

        public virtual DbSet<ParkingCardHistory> ParkingCardHistorie { get; set; }
        public ParkingManagementDBContext(DbContextOptions<ParkingManagementDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Server = parknow.cukvlrirhzja.eu-north-1.rds.amazonaws.com,1433; Database = parkNow; User Id = Admin; Password = test123456;Encrypt=true;TrustServerCertificate=true;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection);
            }
        }
    }
}
