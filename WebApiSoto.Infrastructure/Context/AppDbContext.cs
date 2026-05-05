using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSoto.Application.Interfaces;
using WebApiSoto.Domain.Models;
using WebApiSoto.Infrastructure.Repositories;

namespace WebApiSoto.Infrastructure.Context
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

      

        public DbSet<Users> Users { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(cfg => cfg.HasKey(src => src.UserId));
            modelBuilder.Entity<Categories>(cfg => cfg.HasKey(src => src.Id));
            modelBuilder.Entity<Supplier>(cfg => cfg.HasKey(src => src.SupplierId));
            modelBuilder.Entity<Products>(cfg => cfg.HasKey(src => src.ProductId));
            modelBuilder.Entity<Customers>(cfg => cfg.HasKey(src => src.CustomerId));   
        }
       
        
    }
}
