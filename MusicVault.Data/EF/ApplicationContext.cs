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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //автогенерация значения столбца при добавлении (конкретно дата создания сущности в таблице)
            modelBuilder.Entity<User>().Property(d => d.Create)
                .HasDefaultValueSql("GETDATE()");

            //для таблицы токенов
            modelBuilder.Entity<RefreshToken>().Property(d => d.Created)
                .HasDefaultValueSql("GETDATE()");

            //для плей листов
            modelBuilder.Entity<PlayList>().Property(d => d.Created)
                .HasDefaultValueSql("GETDATE()");

            // настриваем каскадное удаление ,чтоб не было строк с внешним ключом равным NULL
            modelBuilder.Entity<User>()
                .HasMany(p => p.PlayLists)
                .WithOne() //так как у нас теневой ключ то тут просто ничего не вписываем
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
