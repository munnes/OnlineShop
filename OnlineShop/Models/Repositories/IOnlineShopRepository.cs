using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public interface IOnlineShopRepository<TEntity>
    {
        IList<TEntity> list();
        TEntity Find(int id);
        void Add(TEntity entity);
        void Update(int id, TEntity entity);
        void Delete(int id);
        List<TEntity> Search(string term);
       
    }
}
