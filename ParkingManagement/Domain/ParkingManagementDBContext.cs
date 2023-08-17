
using Microsoft.EntityFrameworkCore;
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
    }
}
