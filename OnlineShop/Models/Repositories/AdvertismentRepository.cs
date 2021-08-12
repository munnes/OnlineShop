using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class AdvertismentRepository : IOnlineShopRepository<Advertisment>
    {
        onlineShopContext db;
        public AdvertismentRepository(onlineShopContext _db)
        {
            db = _db;
        }

        public void Add(Advertisment advertisment)
        {
            db.Advertisment.Add(advertisment);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var advertisment = Find(id);
            db.Advertisment.Remove(advertisment);
            db.SaveChanges();
        }

        public Advertisment Find(int id)
        {
            var advertisment = db.Advertisment.SingleOrDefault(adv => adv.AdvId == id);
            return advertisment;
        }

        public IList<Advertisment> list()
        {
            return db.Advertisment.ToList();
        }

        public List<Advertisment> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Advertisment newAdvertisment)
        {
            db.Advertisment.Update(newAdvertisment);
            db.SaveChanges();
        }
    }
}
