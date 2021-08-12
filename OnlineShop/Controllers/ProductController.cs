using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class ProductController : Controller
    {
        private readonly IOnlineShopRepository<Product> productsRepository;
        private readonly IOnlineShopRepository<Category> categoriesRepository;
        private readonly IOnlineShopRepository<PrdByCat> prdByCatsRepository;
        private readonly IOnlineShopRepository<PrdInShop> prdInShopRepository;

        public ProductController(IOnlineShopRepository<Product> productsRepository,
            IOnlineShopRepository<Category> categoriesRepository,
           IOnlineShopRepository<PrdByCat> prdByCatsRepository,
           IOnlineShopRepository<PrdInShop> prdInShopRepository)
        {
            this.productsRepository = productsRepository;
            this.categoriesRepository = categoriesRepository;
            this.prdByCatsRepository = prdByCatsRepository;
            this.prdInShopRepository = prdInShopRepository;
        }


        // GET: ProductController
        public ActionResult Index()
        {
            var prdByCat = prdByCatsRepository.list();
            return View(prdByCat);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var prdByCat = prdByCatsRepository.Find(id);
            //ViewBag.Decode = HttpUtility.HtmlDecode(prdByCat.Description);
   
            return View(prdByCat);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var model = new ProductCategoryViewModel
            {
                Categories = FillCategories()
            };
            return View(model);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategoryViewModel model)
        {
            if (!IsPrdIdUsed(model.PrdCode, 0))
            {
                Product product = new Product
                {
                    PrdId = model.PrdId,
                    PrdCode = model.PrdCode,
                    PrdName = model.PrdName,
                    CatId = model.CatId,
                    Description = model.Description

                };
                productsRepository.Add(product);
                return RedirectToAction(nameof(Index));
            }
            else
            {

                var model2 = new ProductCategoryViewModel
                {
                    Categories = FillCategories()
                };
                return View(model2);
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = productsRepository.Find(id);
            // int catId = product.CatId == 0 ? product.CatId = 0 : product.CatId;

            var viewModel = new ProductCategoryViewModel
            {
                PrdId = product.PrdId,
                PrdCode = product.PrdCode,
                PrdName = product.PrdName,
                CatId = product.CatId,
                Categories = FillCategories(),
                Description = product.Description
            };
            return View(viewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategoryViewModel viewModel)
        {

            if (!IsPrdIdUsed(viewModel.PrdCode, viewModel.PrdId))
            {
                var product = new Product
                {
                    PrdId = viewModel.PrdId,
                    PrdCode = viewModel.PrdCode,
                    PrdName = viewModel.PrdName,
                    CatId = viewModel.CatId,
                    Description = viewModel.Description
                };
                productsRepository.Update(viewModel.PrdId, product);
                return RedirectToAction(nameof(Index));
            }
            else
            {

                var viewModel2 = new ProductCategoryViewModel
                {
                    PrdId = viewModel.PrdId,
                    PrdCode = viewModel.PrdCode,
                    PrdName = viewModel.PrdName,
                    CatId = viewModel.CatId,
                    Categories = FillCategories(),
                    Description = viewModel.Description
                };
                return View(viewModel2);

            }

        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var prdByCat = prdByCatsRepository.Find(id);
            //ViewBag.Decode = HttpUtility.HtmlDecode(prdByCat.Description);
            return View(prdByCat);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            if (canDelPrd(id))
            {
                productsRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var prdByCat = prdByCatsRepository.Find(id);
                //ViewBag.Decode = HttpUtility.HtmlDecode(prdByCat.Description);
                return View(prdByCat);
            }
        }
        /***********************************************************/
        List<Category> FillCategories()
        {
            var categories = categoriesRepository.list().ToList();
            categories.Insert(0, new Category { CatId = 0, CatName = "...Please select category" });
            return categories;
        }
        bool IsPrdIdUsed(string id, int trk)
        {
            //'Product' cannot be tracked because another 
            //instance with the same key value for {'Trk'} is already being tracked
            //so i use==> prdByCatsRepository
            var products = prdByCatsRepository.list();
            var myProduct = products.SingleOrDefault(p => p.PrdCode == id && p.PrdId != trk);
            if (myProduct == null)
            {
                return false;
            }
            else
            {
                ViewBag.PID = "Item Code Already Used!!";
                return true;
            }

        }

        bool canDelPrd(int prdId)
        {
            var product = prdInShopRepository.list().Where(p => p.PrdId == prdId).ToList();
            if (product.Count() == 0)
            {
                ViewBag.CntDel = "";
                return true;
            }
            else
            {
                ViewBag.CntDel = "Sorry Product Can't be delete!! it's used in anther place.";
                return false;
            }

        }
        public ActionResult Search(string term)
        {
            var result = prdByCatsRepository.Search(term);
            return View("Index", result);
        }

    
    }

}
