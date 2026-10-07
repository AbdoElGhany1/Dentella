using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using MainDentalla.Models;

namespace Dentella3.Models
{
    public class Appdbcontext : IdentityDbContext
    {
        public Appdbcontext()
        {

        }
        public Appdbcontext(DbContextOptions<Appdbcontext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Doctor and IdentityUser
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .IsRequired(false);
            modelBuilder.Entity<Patient>()
               .HasOne(d => d.User)
               .WithMany()
               .HasForeignKey(d => d.UserId)
               .IsRequired(false);
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Card> Cards { get; set; }


    }
}
