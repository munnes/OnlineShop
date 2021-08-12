using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class AvailStockRepository : IOnlineShopRepository<AvailStock>
    {
        onlineShopContext db;
        public AvailStockRepository(onlineShopContext _db)
        {
            db = _db;
        }

        public void Add(AvailStock entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public AvailStock Find(int id)
        {//search by prdid
            var availStock = db.AvailStock.SingleOrDefault(avSt => avSt.PrdId == id);
            return availStock;
        }

        public IList<AvailStock> list()
        {
            return db.AvailStock.ToList();
        }

        public List<AvailStock> Search(string term)
        {
            if (term !=null)
            {
                var result = db.AvailStock
                 .Where(b => b.PrdName.Contains(term)
                      || b.CatName.Contains(term)).ToList();
                return result;
            }
            else
            {
            return db.AvailStock.ToList();
            }
        }

        public void Update(int id, AvailStock entity)
        {
            throw new NotImplementedException();
        }
    }
}
