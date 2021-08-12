using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{
    public class SecondaryImgRepository : IOnlineShopRepository<SecondaryImg>
    {
        private readonly onlineShopContext db;

        public SecondaryImgRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(SecondaryImg entity)
        {
            db.SecondaryImg.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var secondaryImg = Find(id);
            db.SecondaryImg.Remove(secondaryImg);
            db.SaveChanges();
        }

        public SecondaryImg Find(int id)
        {
            var secondaryImg = db.SecondaryImg.SingleOrDefault(sImg => sImg.ImgId == id);
            return secondaryImg;
        }

        public IList<SecondaryImg> list()
        {
            return db.SecondaryImg.ToList();
        }

        public List<SecondaryImg> Search(string term)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, SecondaryImg newImg)
        {
            db.SecondaryImg.Update(newImg);
            db.SaveChanges();
        }
    }
}
