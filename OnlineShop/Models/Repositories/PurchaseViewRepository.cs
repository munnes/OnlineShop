using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class PurchaseViewRepository : IOnlineShopRepository<PurchaseView>
    {
        private readonly onlineShopContext db;

        public PurchaseViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(PurchaseView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PurchaseView Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<PurchaseView> list()
        {
         return  db.PurchaseView.ToList();
        }

        public List<PurchaseView> Search(string term)
        {
            if (term != null)
            {
                return db.PurchaseView.Where(p => p.NameId.Contains(term)).ToList();
            }
            else
            {
                return db.PurchaseView.ToList();
            }
            }

        public void Update(int id, PurchaseView entity)
        {
            throw new NotImplementedException();
        }
    }
}
