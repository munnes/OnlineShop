using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class ReviewsRepository : IOnlineShopRepository<Reviews>
    {
        private readonly onlineShopContext db;

        public ReviewsRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(Reviews entity)
        {
            db.Reviews.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var review = Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
        }

        public Reviews Find(int id)
        {
            var review = db.Reviews.SingleOrDefault(rv => rv.RId == id);
            return review;
        }

        public IList<Reviews> list()
        {
            return db.Reviews.ToList();
        }

        public List<Reviews> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Reviews newReview)
        {
            db.Reviews.Update(newReview);
            db.SaveChanges();
        }
    }
}
