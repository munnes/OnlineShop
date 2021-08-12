using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class PrdByCatRepository : IOnlineShopRepository<PrdByCat>
    {
        onlineShopContext db;
        public PrdByCatRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(PrdByCat entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PrdByCat Find(int id)
        {
            var prdByCat = db.PrdByCat.SingleOrDefault(p => p.PrdId == id);
            return prdByCat;
        }

        public IList<PrdByCat> list()
        {
            return (IList<PrdByCat>)db.PrdByCat.ToList();
        }

        public List<PrdByCat> Search(string term)
        {
            return db.PrdByCat.Where(p => p.PrdName.Contains(term)
            ||p.CatName.Contains(term)||p.PrdCode.Contains(term)).ToList();
        }

        public void Update(int id, PrdByCat entity)
        {
            throw new NotImplementedException();
        }
    }
}
