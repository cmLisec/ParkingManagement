﻿using Microsoft.EntityFrameworkCore;
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

        public virtual DbSet<ParkingCardHistory> ParkingCardHistory { get; set; }

        public virtual DbSet<CardDetails> CardDetails { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<PaymentTransaction> PaymentTransaction { get; set; }
        public DbSet<ExpenseHistory> ExpenseHistories { get; set; }
        public DbSet<SettleUpHistory> SettleUpHistories { get; set; }
        public ParkingManagementDBContext(DbContextOptions<ParkingManagementDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Server=parkingmanagement.postgres.database.azure.com;Database=postgres;Port=5432;User Id=superUser;Password=admin@123;Ssl Mode=Require;Trust Server Certificate=true;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connection);
            }
        }
    }
}
