using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class AdvertismentViewRepository : IOnlineShopRepository<AdvertismentView>
    {
        onlineShopContext db;
        public AdvertismentViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(AdvertismentView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public AdvertismentView Find(int id)
        {
            var advertismentView = db.AdvertismentView.SingleOrDefault(adv => adv.AdvId == id);
            return advertismentView;
        }

        public IList<AdvertismentView> list()
        {
            return db.AdvertismentView.ToList();
        }

        public List<AdvertismentView> Search(string term)
        {
           var result = db.AdvertismentView
               .Where(b => b.PrdName.Contains(term)
                    || b.CatName.Contains(term)).ToList();
            return result;
        }

        public void Update(int id, AdvertismentView entity)
        {
            throw new NotImplementedException();
        }
    }
}
