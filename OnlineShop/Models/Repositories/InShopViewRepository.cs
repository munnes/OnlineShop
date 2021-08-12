using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class InShopViewRepository : IOnlineShopRepository<InShopView>
    {
        onlineShopContext db;
        public InShopViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(InShopView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public InShopView Find(int id)
        {
            var inShopView = db.InShopView.SingleOrDefault(InShop => InShop.Trk == id);
            return inShopView;
        }

        public IList<InShopView> list()
        {
            return db.InShopView.ToList();
        }

        public List<InShopView> Search(string term)
        {
            var result = db.InShopView
              .Where(b => b.PrdName.Contains(term)
                   || b.CatName.Contains(term)).ToList();
            return result;
        }

        public void Update(int id, InShopView entity)
        {
            throw new NotImplementedException();
        }
    }
}
