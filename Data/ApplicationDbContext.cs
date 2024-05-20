using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpressVoitures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<CarBrand> Marques { get; set; }
        public DbSet<CarModel> Modeles { get; set; }
        public DbSet<CarFinish> Finitions { get; set; }
        public DbSet<CarImage> VehicleImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vehicle>().Property(v => v.CoutsReparation).HasColumnType("decimal(8, 2)");
            modelBuilder.Entity<Vehicle>().Property(v => v.PrixAchat).HasColumnType("decimal(8, 2)");
            modelBuilder.Entity<Vehicle>().Property(v => v.PrixVente).HasColumnType("decimal(8, 2)");
            modelBuilder.Entity<Vehicle>().Property(v => v.Annee).HasColumnType("char(4)").IsRequired();

            modelBuilder.Entity<CarModel>()
                .HasOne(m => m.Marque)
                .WithMany(mq => mq.Modeles)
                .HasForeignKey(m => m.MarqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CarFinish>()
                .HasOne(f => f.Modele)
                .WithMany(m => m.Finitions)
                .HasForeignKey(f => f.ModeleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Marque)
                .WithMany()
                .HasForeignKey(v => v.MarqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Modele)
                .WithMany()
                .HasForeignKey(v => v.ModeleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Finition)
                .WithMany()
                .HasForeignKey(v => v.FinitionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
