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
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //автогенерация значения столбца при добавлении (конкретно дата создания сущности в таблице)
            modelBuilder.Entity<User>().Property(d=>d.Create)
                .HasDefaultValueSql("GETDATE()");



            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User{Id=System.Guid.NewGuid(),Email="greatdragone75@gmail.com",Login="TestUser1",Password="123456"}
            });
        }
    }
}
