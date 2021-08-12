using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class SecondaryImgViewRepository : IOnlineShopRepository<SecondaryImgView>
    {
        private readonly onlineShopContext db;

        public SecondaryImgViewRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(SecondaryImgView entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public SecondaryImgView Find(int id)
        {
            var secImgview = db.SecondaryImgView.SingleOrDefault(i => i.ImgId == id);
            return secImgview;
        }

        public IList<SecondaryImgView> list()
        {
            return db.SecondaryImgView.ToList();
        }

        public List<SecondaryImgView> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, SecondaryImgView entity)
        {
            throw new NotImplementedException();
        }
    }
}
