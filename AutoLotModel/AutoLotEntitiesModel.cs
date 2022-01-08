using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace AutoLotModel
{
    public partial class AutoLotEntitiesModel : DbContext
    {
        public AutoLotEntitiesModel()
            : base("name=AutoLotEntitiesModel")
        {
        }

        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(e => e.LastName)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Appointments)
                .WithOptional(e => e.Customer)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Service>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Service>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Service>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.Appointments)
                .WithOptional(e => e.Service)
                .WillCascadeOnDelete();
        }
    }
}
