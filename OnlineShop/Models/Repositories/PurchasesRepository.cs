using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class PurchasesRepository : IOnlineShopRepository<Purchases>
    {
        onlineShopContext db;
        public PurchasesRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(Purchases purchases)
        {
            db.Purchases.Add(purchases);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var purchase = Find(id);
            db.Purchases.Remove(purchase);
            db.SaveChanges();
        }

        public Purchases Find(int id)
        {
            var purchase = db.Purchases.SingleOrDefault(pr => pr.TrkOut == id);
            return purchase;
        }

        public IList<Purchases> list()
        {
            return db.Purchases.ToList();
        }

        public List<Purchases> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Purchases newPurchase)
        {
            db.Purchases.Update(newPurchase);
            db.SaveChanges();
        }
    }
}
