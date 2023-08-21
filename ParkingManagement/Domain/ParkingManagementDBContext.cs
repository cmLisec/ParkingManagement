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
            string connection = "Data Source=parknow.database.windows.net;Initial Catalog=ParkNow;User ID=parknowAdmin;Password=parkNow@123;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connection);
            }
        }
    }
}
