using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class OrdersRepository : IOnlineShopRepository<Orders>
    {
        onlineShopContext db;
        public OrdersRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(Orders order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
        }

        public Orders Find(int id)
        {
            var order=db.Orders.SingleOrDefault(o => o.OrdId == id);
            return order;
        }

        public IList<Orders> list()
        {
            return db.Orders.ToList();
        }

        public List<Orders> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, Orders newOrder)
        {
            db.Orders.Update(newOrder);
            db.SaveChanges();
        }
    }
}
