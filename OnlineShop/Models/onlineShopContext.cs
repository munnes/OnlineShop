using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlineShop.ViewModel;

namespace OnlineShop.Models
{
    public partial class onlineShopContext : DbContext
    {
        public onlineShopContext()
        {
        }

        /*  public onlineShopContext()
 {
 }*/

        public onlineShopContext(DbContextOptions<onlineShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<PrdInShop> PrdInShop { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Purchases> Purchases { get; set; }
        public virtual DbSet<Advertisment> Advertisment { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<SecondaryImg> SecondaryImg { get; set; }
        public DbQuery<PrdByCat> PrdByCat { get; set; } //this can be done to use SQL view
                                                        // public object Products { get; internal set; }
        public DbQuery<InShopView> InShopView { get; set; }
        public DbQuery<AvailStock> AvailStock { get; set; }
        public DbQuery<AvlCategory> AvlCategory { get; set; }
        public DbQuery<AvlProduct> AvlProduct { get; set; }
        public DbQuery<AdvertismentView> AdvertismentView { get; set; }
        public DbQuery<PrchFRecView> PrchFRecView { get; set; }
        public DbQuery<UsrTrans> UsrTrans { get; set; }
        public DbQuery<CouriersView> CouriersView { get; set; }
        public DbQuery<PurchaseView> PurchaseView { get; set; }
        public DbQuery<ReviewsView> ReviewsView { get; set; }
        public DbQuery<SecondaryImgView> SecondaryImgView { get; set; }
   
    }
}