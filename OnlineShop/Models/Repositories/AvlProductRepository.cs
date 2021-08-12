using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class AvlProductRepository : IOnlineShopRepository<AvlProduct>
    {
        onlineShopContext db;
        public AvlProductRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(AvlProduct entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public AvlProduct Find(int id)
        {
            var avlProduct = db.AvlProduct.SingleOrDefault(p => p.PrdId == id);
            return avlProduct;
        }

        public IList<AvlProduct> list()
        {
            return db.AvlProduct.ToList();
        }

        public List<AvlProduct> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, AvlProduct entity)
        {
            throw new NotImplementedException();
        }
    }
}
