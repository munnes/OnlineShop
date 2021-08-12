
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class ProductsRepository : IOnlineShopRepository<Product>
    {
        onlineShopContext db;
        public ProductsRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(Product entity)
        {
            db.Product.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
        }

        public Product Find(int id)
        {
            var product = db.Product.SingleOrDefault(p => p.PrdId == id);

            return product;
        }

        public IList<Product> list()
        {
            return db.Product.ToList();
        }

        public List<Product> Search(string term)
        {
            return db.Product.Where(p => p.PrdName.Contains(term)).ToList();
        }

        public void Update(int id, Product newProduct)
        {
            db.Product.Update(newProduct);
            db.SaveChanges();
        }


    }
}
