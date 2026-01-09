using Microsoft.EntityFrameworkCore;
using MindMapApp.Entities;
using System.Collections.Generic;

namespace MindMapAppAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<MindMap> MindMaps { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MindMapDb;Trusted_Connection=True;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.FromNode)
                .WithMany()
                .HasForeignKey(c => c.FromNodeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Connection>()
                .HasOne(c => c.ToNode)
                .WithMany()
                .HasForeignKey(c => c.ToNodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}