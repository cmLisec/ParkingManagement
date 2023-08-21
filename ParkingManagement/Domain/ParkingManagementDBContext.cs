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
            string connection = "Server=tcp:parknow.database.windows.net,1433;Initial Catalog=ParkNow;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection);
            }
        }
    }
}
