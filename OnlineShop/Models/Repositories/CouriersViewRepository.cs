using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class CouriersViewRepository : IOnlineShopRepository<CouriersView>
    {
        private readonly onlineShopContext db;

        public CouriersViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(CouriersView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public CouriersView Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<CouriersView> list()
        {
            return db.CouriersView.ToList();
        }

        public List<CouriersView> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, CouriersView entity)
        {
            throw new NotImplementedException();
        }
    }
}
