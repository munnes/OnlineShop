using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using OnlineShop.Classes;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class PrdInShopController : Controller
    {
        private readonly IOnlineShopRepository<InShopView> inShopViewRepository;
        private readonly IOnlineShopRepository<PrdInShop> prdInShopRepository;
        private readonly IOnlineShopRepository<Product> productRepository;
        private readonly IOnlineShopRepository<Category> categoriesRepository;
        private readonly IOnlineShopRepository<PrdByCat> prdByCatsRepository;
        private readonly IOnlineShopRepository<Purchases> purchasesRepository;
        private readonly IOnlineShopRepository<AvailStock> availStockRepository;
        private readonly IHostingEnvironment hosting;

        public PrdInShopController(IOnlineShopRepository<InShopView> inShopViewRepository,
                            IOnlineShopRepository<PrdInShop> prdInShopRepository,
                            IOnlineShopRepository<Product> productRepository,
                            IOnlineShopRepository<Category> categoriesRepository,
                            IOnlineShopRepository<PrdByCat> prdByCatsRepository,
                            IOnlineShopRepository<Purchases> purchasesRepository,
                            IOnlineShopRepository<AvailStock> availStockRepository,
                            IHostingEnvironment hosting)
        {
            this.inShopViewRepository = inShopViewRepository;
            this.prdInShopRepository = prdInShopRepository;
            this.productRepository = productRepository;
            this.categoriesRepository = categoriesRepository;
            this.prdByCatsRepository = prdByCatsRepository;
            this.purchasesRepository = purchasesRepository;
            this.availStockRepository = availStockRepository;
            this.hosting = hosting;
        }
        // GET: PrdInShopController
        public ActionResult Index()
        {
            var pInShop = inShopViewRepository.list();
            return View(pInShop);
        }

        // GET: PrdInShopController/Details/5
        public ActionResult Details(int id)
        {
            var pInShop = inShopViewRepository.Find(id);
            return View(pInShop);
        }

        // GET: PrdInShopController/Create
        public ActionResult Create()
        {
            var model = new ProductInShopViewModel
            {
                Categories = categoriesRepository.list().ToList(),
                DateAdd = DateTime.Today
            };

            return View(model);
        }
        /********************************************/

        public JsonResult GetProducts(int catId)
        {// used in json => Jquery
            var prdByCat = prdByCatsRepository.list().Where(p => p.CatId == catId).ToList();

            var prodLists = new List<ProdList> { };
            for (var i = 0; i < prdByCat.Count; i++)
            {
                prodLists.Add(new ProdList { PrdId = prdByCat[i].PrdId, NameId = prdByCat[i].NameId });
            }

            return Json(prodLists);
        }
        /****************************************/

        // POST: PrdInShopController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductInShopViewModel model)
        {
            /*******/

            string fileName = UploadFile(model.File) ?? string.Empty;

            if (ModelState.IsValid && fileName != string.Empty)
            {
                PrdInShop prdInShop = new PrdInShop
                {

                    PrdId = model.PrdId,
                    DateAdd = model.DateAdd,
                    Qty = model.Qty,
                    Price = model.Price,
                    Pic = fileName

                };
                prdInShopRepository.Add(prdInShop);
                return RedirectToAction(nameof(Index));
            }
            else
            {

                var model2 = new ProductInShopViewModel
                {
                    Categories = categoriesRepository.list().ToList(),
                    PrdId = model.PrdId// should be added to save when refresh after create and no img
                };
                return View(model2);
            }
            /**********/


        }

        // GET: PrdInShopController/Edit/5
        public ActionResult Edit(int id)
        {
            var prdInShop = prdInShopRepository.Find(id);
            var prd = prdByCatsRepository.Find(prdInShop.PrdId);
            ViewBag.OldQty = prdInShop.Qty;
            ProductInShopViewModel viewModel = new ProductInShopViewModel
            {
                Trk = prdInShop.Trk,
                Categories = categoriesRepository.list().ToList(),
                saveQty = prdInShop.Qty,
                Qty = prdInShop.Qty,
                Price = prdInShop.Price,
                DateAdd = prdInShop.DateAdd,
                Pic = prdInShop.Pic,
                PrdId = prdInShop.PrdId,
                CatId = prd.CatId
            };

            return View(viewModel);
        }

        // POST: PrdInShopController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductInShopViewModel viewModel)
        {
            bool chkEdit=canEdit(viewModel.PrdId, viewModel.saveQty, viewModel.Qty);
            if (ModelState.IsValid && chkEdit)
            {
                
                string fileName = UploadFile(viewModel.File, viewModel.Pic, viewModel.Trk);

                PrdInShop prdInShop = new PrdInShop
                {
                    Trk = viewModel.Trk,
                    PrdId = viewModel.PrdId,
                    Qty = viewModel.Qty,
                    Price = viewModel.Price,
                    DateAdd = viewModel.DateAdd,
                    Pic = fileName
                };
                prdInShopRepository.Update(viewModel.Trk, prdInShop);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var model2 = new ProductInShopViewModel
                {
                    Categories = categoriesRepository.list().ToList(),
                    PrdId = viewModel.PrdId,// should be added to save when refresh after create and no img
                    DateAdd = viewModel.DateAdd,
                    Pic = viewModel.Pic
                };

                return View(model2);
            }
        }

        // GET: PrdInShopController/Delete/5
        public ActionResult Delete(int id)
        {
            var pInShop = inShopViewRepository.Find(id);
            return View(pInShop);
        }

        // POST: PrdInShopController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, InShopView inShopView)
        {
           
                if (canDelPrd(id))
                { 
                prdInShopRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
           else
            {
                var inShop = inShopViewRepository.Find(id);
                return View(inShop);
            }
        }

        //**************************
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads, file.FileName);
                // close the streem , because without that it will give file used by another process
                var theStream = new FileStream(fullPath, FileMode.Create);
                file.CopyTo(theStream);
                theStream.Close();

                return file.FileName;
            }
            ViewBag.ImgUrl = "The Image field is required!!";
            return null;
        }
        string UploadFile(IFormFile file, string imageUrl, int trk)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string newPath = Path.Combine(uploads, file.FileName);
                string oldPath = Path.Combine(uploads, imageUrl);
                if (oldPath != newPath)
                {
                    if (IsImgUploaded(imageUrl, trk) == false)
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    //save the new file
                    var theStream = new FileStream(newPath, FileMode.Create);
                    file.CopyTo(theStream);
                    theStream.Close();
                }

                return file.FileName;
            }
            return imageUrl;

        }
        bool IsImgUploaded(string imageUrl, int trk)
        {
            var inShop = inShopViewRepository.list()
                .Where(prd => prd.Pic == imageUrl && prd.Trk != trk).ToList();
            if (inShop.Count == 0)
            {
                return false;

            }
            else
            {
                return true;
            }
        }
        public ActionResult Search(string term)
        {
            var result = inShopViewRepository.Search(term);
            return View("Index", result);
        }
        bool canDelPrd(int prdId)
        {
            var product = availStockRepository.list().Where(p => p.PrdId == prdId ).ToList();
            if (product.Count() == 0)
            {
                ViewBag.CntDel = "";
                return true;
            }
            else
            {
                ViewBag.CntDel = "Sorry Product Can't be delete!! it's used in another place!!";
                return false;
            }

        }
        bool canEdit(int prdId,int oldQty,int newQty)
        {
            var availStock = availStockRepository.Find(prdId);
            var qty = availStock.AvlQty - oldQty + newQty;
            if(qty<0)
            {
                ViewBag.CntEdit = "Sorry Product Quantity can't be decreased to this value!!";
                return false;
            }
            else
            {
                ViewBag.CntEdit = "";
                return true;
            }
        }

    }
}
