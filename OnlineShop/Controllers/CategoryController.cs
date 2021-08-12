using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class CategoryController : Controller
    {
        private readonly IOnlineShopRepository<Category> categoryRepository;
        private readonly IOnlineShopRepository<Product> productRepository;

        public CategoryController(IOnlineShopRepository<Category> categoryRepository,
            IOnlineShopRepository<Product> productRepository)
        {
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }
        // GET: CategoryController
 
        public ActionResult Index()
        {
            var catergories = categoryRepository.list();
            return View(catergories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = categoryRepository.Find(id);
            ViewBag.Decode = HttpUtility.HtmlDecode(category.mainDesc);
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {

            try
            {
                categoryRepository.Add(category);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var category = categoryRepository.Find(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                categoryRepository.Update(id, category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var category = categoryRepository.Find(id);
            ViewBag.Decode = HttpUtility.HtmlDecode(category.mainDesc);
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (canDelCat(id))
            {
                categoryRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var category = categoryRepository.Find(id);
                ViewBag.Decode = HttpUtility.HtmlDecode(category.mainDesc);
                return View(category);
            }
        }
        bool canDelCat(int catId)
        {
            var product = productRepository.list().Where(p => p.CatId == catId).ToList();
            if (product.Count() == 0)
            {
                ViewBag.CntDel = "";
                return true;
            }
            else
            {
                ViewBag.CntDel = "Sorry Categrory Can't be delete!! it's used in anther place.";
                return false;
            }

        }
        public ActionResult Search(string term)
        {
            var result = categoryRepository.Search(term);
            return View("Index", result);
        }
    }
}
