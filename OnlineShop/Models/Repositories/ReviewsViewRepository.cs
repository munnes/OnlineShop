using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class ReviewsViewRepository : IOnlineShopRepository<ReviewsView>
    {
        private readonly onlineShopContext db;

        public ReviewsViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(ReviewsView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ReviewsView Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ReviewsView> list()
        {
            return db.ReviewsView.ToList();
        }

        public List<ReviewsView> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, ReviewsView entity)
        {
            throw new NotImplementedException();
        }
    }
}
