using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models.Repositories
{

    public class CategoriesRepository : IOnlineShopRepository<Category>
    {
        onlineShopContext db;
        public CategoriesRepository(onlineShopContext _db)
        {
            db = _db;
        }
        public void Add(Category category)
        {
            db.Category.Add(category);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = Find(id);
            db.Category.Remove(category);
            db.SaveChanges();
        }

        public void Update(int id, Category newCategory)
        {
            db.Category.Update(newCategory);
            db.SaveChanges();
        }

        public Category Find(int id)
        {
            var category = db.Category.SingleOrDefault(c => c.CatId == id);
            return category;
        }

        public IList<Category> list()
        {
            return db.Category.ToList();
        }

        public List<Category> Search(string term)
        {
            return db.Category.Where(c => c.CatName.Contains(term)).ToList();
        }
    }
}
