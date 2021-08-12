using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class PrchFRecViewRepository : IOnlineShopRepository<PrchFRecView>
    {
        onlineShopContext db;
        public PrchFRecViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(PrchFRecView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PrchFRecView Find(int id)
        {
           var prchFRecView =db.PrchFRecView.SingleOrDefault(pr=>pr.TrkOut==id);
            return prchFRecView;
        }

        public IList<PrchFRecView> list()
        {
            return db.PrchFRecView.ToList();
        }

        public List<PrchFRecView> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, PrchFRecView entity)
        {
            throw new NotImplementedException();
        }
    }
}
