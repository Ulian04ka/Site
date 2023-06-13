using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopKnitting.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopKnitting.Models.HelpModels
{
    public class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public DbSet<ProductModel> ProductModels { get; set; }
        public DbSet<OrderModel> OrderModel { get; set; }
        public DbSet<ImageModel> ImageModel { get; set; }
        public DbSet<CarDealershipModel> CarDealershipModel { get; set; }
        public DbSet<BrandModel> BrandModel { get; set; }
        public DbSet<BasketModel> BasketModel { get; set; }
        public DbSet<BasketProductLinkModel> BasketProductLinkModel { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BasketProductLinkModel>(entity =>
            {
                entity.HasKey(e => new { e.BasketId, e.ProductId });
            });
        }
    }
}
