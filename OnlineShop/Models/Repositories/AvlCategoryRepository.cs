using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class AvlCategoryRepository : IOnlineShopRepository<AvlCategory>
    {
        onlineShopContext db;
        public AvlCategoryRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(AvlCategory entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public AvlCategory Find(int id)
        {
            var avlCategory = db.AvlCategory.SingleOrDefault(c => c.CatId == id);
            return avlCategory;
        }

        public IList<AvlCategory> list()
        {
            return db.AvlCategory.ToList();
        }

        public List<AvlCategory> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, AvlCategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
