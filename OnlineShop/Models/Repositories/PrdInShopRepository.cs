using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class PrdInShopRepository : IOnlineShopRepository<PrdInShop>
    {
        onlineShopContext db;
        public PrdInShopRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(PrdInShop entity)
        {
            db.PrdInShop.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var prdInShop = Find(id);
            db.PrdInShop.Remove(prdInShop);
            db.SaveChanges();
        }

        public PrdInShop Find(int id)
        {

            var prdInShop = db.PrdInShop.SingleOrDefault(pIs => pIs.Trk == id);
            return prdInShop;
        }

        public IList<PrdInShop> list()
        {
            return db.PrdInShop.ToList();
        }

        public List<PrdInShop> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, PrdInShop newPrdInShop)
        {
            db.PrdInShop.Update(newPrdInShop);
            db.SaveChanges();
        }
    }
}
