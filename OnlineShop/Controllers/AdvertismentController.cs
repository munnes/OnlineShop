using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Classes;
using OnlineShop.Models;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdvertismentController : Controller
    {
        private readonly IOnlineShopRepository<Advertisment> advertismentRepository;
        private readonly IOnlineShopRepository<AvlCategory> avlCategoriesRepository;
        private readonly IOnlineShopRepository<AvlProduct> avlProductRepository;
        private readonly IOnlineShopRepository<AvailStock> availStockRepository;
        private readonly IOnlineShopRepository<AdvertismentView> advertismentViewRepository;

        public AdvertismentController(IOnlineShopRepository<Advertisment> advertismentRepository,
            IOnlineShopRepository<AvlCategory> avlCategoriesRepository,
            IOnlineShopRepository<AvlProduct> avlProductRepository,
           IOnlineShopRepository<AvailStock> availStockRepository,
           IOnlineShopRepository<AdvertismentView> advertismentViewRepository)
        {
            this.advertismentRepository = advertismentRepository;
            this.avlCategoriesRepository = avlCategoriesRepository;
            this.avlProductRepository = avlProductRepository;
            this.availStockRepository = availStockRepository;
            this.advertismentViewRepository = advertismentViewRepository;
        }
        public ActionResult Index()
        {
            var advertismentsView = advertismentViewRepository.list();

            return View(advertismentsView);
        }

        public ActionResult Create()
        {
            var model = new ProductAdvViewModel
            {
                Categories = avlCategoriesRepository.list().ToList(),
                DFrom = DateTime.Today,
                DTo = DateTime.Today.Date.AddDays(7)

            };

            return View(model);
        }
        public JsonResult GetProducts(int catId)
        {// used in json => Jquery
            var prdByCat = avlProductRepository.list().Where(p => p.CatId == catId).ToList();

            var prodLists = new List<ProdList> { };
            for (var i = 0; i < prdByCat.Count; i++)
            {
                prodLists.Add(new ProdList { PrdId = prdByCat[i].PrdId, NameId = prdByCat[i].NameId });
            }

            return Json(prodLists);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductAdvViewModel model)
        {
            /*******/

            if (ModelState.IsValid)
            {
                Advertisment advertisment = new Advertisment
                {
                    PrdId = model.PrdId,
                    AdvTxt = model.AdvTxt,
                    AdvTitle = model.AdvTitle,
                    DFrom = model.DFrom,
                    DTo = model.DTo
                };
                advertismentRepository.Add(advertisment);
                return RedirectToAction(nameof(Index));
            }
            else
            {

                var model2 = new ProductAdvViewModel
                {
                    Categories = avlCategoriesRepository.list().ToList(),
                    PrdId = model.PrdId// should be added to save when refresh after create and no img
                };
                return View(model2);
            }
            /**********/
        }


        // GET: PrdInShopController/Edit/5
        public ActionResult Edit(int id)
        {
            var advertisment = advertismentRepository.Find(id);
            // var prd = prdByCatsRepository.Find(prdInShop.PrdId);
            var prd = avlProductRepository.Find(advertisment.PrdId);

            ProductAdvViewModel viewModel = new ProductAdvViewModel
            {
                AdvId = advertisment.AdvId,
                PrdId = advertisment.PrdId,
                Categories = avlCategoriesRepository.list().ToList(),
                CatId = prd.CatId,
                AdvTitle=advertisment.AdvTitle,
                AdvTxt = advertisment.AdvTxt,
                DFrom = advertisment.DFrom,
                DTo = advertisment.DTo
            };

            return View(viewModel);
        }

        // POST: PrdInShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductAdvViewModel viewModel)
        {

            if (ModelState.IsValid)
            {


                Advertisment advertisment = new Advertisment
                {
                    AdvId = viewModel.AdvId,
                    PrdId = viewModel.PrdId,
                    AdvTxt = viewModel.AdvTxt,
                    AdvTitle=viewModel.AdvTitle,
                    DFrom = viewModel.DFrom,
                    DTo = viewModel.DTo
                };
                advertismentRepository.Update(viewModel.AdvId, advertisment);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var model2 = new ProductAdvViewModel
                {
                    Categories = avlCategoriesRepository.list().ToList(),
                    PrdId = viewModel.PrdId,// should be added to save when refresh after create and no img
                    DFrom = viewModel.DFrom,
                    DTo = viewModel.DTo
                };

                return View(model2);
            }
        }
        public ActionResult Details(int id)
        {
            var advertismentView = advertismentViewRepository.Find(id);
            return View(advertismentView);
        }
        public ActionResult Delete(int id)
        {
            var advertismentView = advertismentViewRepository.Find(id);
            return View(advertismentView);
        }
        // POST: PrdInShopController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, AdvertismentView advertismentView)
        {
            try
            {
                advertismentRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Search(string term)
        {
                var result = advertismentViewRepository.Search(term);
                return View("Index", result);
        }

    }
}
