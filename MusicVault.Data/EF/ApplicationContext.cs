using Microsoft.EntityFrameworkCore;
using MusicVault.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicVault.Data.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //автогенерация значения столбца при добавлении (конкретно дата создания сущности в таблице)
            modelBuilder.Entity<User>().Property(d=>d.Create)
                .HasDefaultValueSql("GETDATE()");



            
        }
    }
}
