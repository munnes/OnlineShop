using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class UsrTransRepository : IOnlineShopRepository<UsrTrans>
    {
        onlineShopContext db;
        public UsrTransRepository (onlineShopContext _db)
        {
            db=_db;
        }
        public void Add(UsrTrans entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UsrTrans Find(int id)
        {
            var usrTran = db.UsrTrans.SingleOrDefault(ord => ord.TrkOut == id);
            return usrTran;
        }

        

        public IList<UsrTrans> list()
        {
            return db.UsrTrans.ToList();
        }

        public List<UsrTrans> Search(string term)
        {
            if (term != null)
            {
                return db.UsrTrans.Where(p => p.NameId.Contains(term) || p.CourName.Contains(term)).ToList();
            }
            else
            {
                return db.UsrTrans.ToList();
            }
            }

        public void Update(int id, UsrTrans entity)
        {
            throw new NotImplementedException();
        }
    }
}
