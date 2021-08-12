using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Classes;
using OnlineShop.Models;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class SecondaryImgController : Controller
    {
        private readonly IOnlineShopRepository<SecondaryImg> secondaryImgRepository;
        private readonly IOnlineShopRepository<Product> productRepository;
        private readonly IOnlineShopRepository<Category> categoryRepository;
        private readonly IOnlineShopRepository<PrdByCat> prdByCatRepository;
        private readonly IOnlineShopRepository<SecondaryImgView> secondaryImgViewRepository;
        private readonly IHostingEnvironment hosting;

        public SecondaryImgController(IOnlineShopRepository<SecondaryImg> secondaryImgRepository,
            IOnlineShopRepository<Product> productRepository,
            IOnlineShopRepository<Category> categoryRepository,
            IOnlineShopRepository<PrdByCat> prdByCatRepository,
            IOnlineShopRepository<SecondaryImgView> secondaryImgViewRepository,
            IHostingEnvironment hosting)
        {
            this.secondaryImgRepository = secondaryImgRepository;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.prdByCatRepository = prdByCatRepository;
            this.secondaryImgViewRepository = secondaryImgViewRepository;
            this.hosting = hosting;
        }
        // GET: SecondaryImgController
        public ActionResult Index()
        {
            SecImgViewModel secImgViewModel = new SecImgViewModel();
            secImgViewModel.Category = categoryRepository.list().ToList();
            secImgViewModel.SecondaryImg = secondaryImgRepository.list().ToList();
          //  secImgViewModel.Product = productRepository.list().ToList();
            return View(secImgViewModel);
        }

        // GET: SecondaryImgController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SecondaryImgController/Create
        public ActionResult Create()
        {
            SecImgViewModel secImgViewModel = new SecImgViewModel();
          //  secImgViewModel.Product = productRepository.list().ToList();
            secImgViewModel.Category = categoryRepository.list().ToList();
            return View(secImgViewModel);
        }

        // POST: SecondaryImgController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SecImgViewModel model)
        {
           string fileName = UploadFile(model.File) ?? string.Empty;
 
            var secOfPrd = (from s in secondaryImgRepository.list() where s.PrdId == model.PrdId select s.ImgId).ToList();
            int noOfImg = secOfPrd.Count();
          
                if (ModelState.IsValid && fileName != string.Empty && noOfImg < 5)
                {
                    SecondaryImg secondaryImg = new SecondaryImg
                    {
                        PrdId = model.PrdId,
                        ImgId = model.ImgId,
                        CatId=model.CatId,
                        SecPic = fileName
                    };
                    secondaryImgRepository.Add(secondaryImg);
                    return RedirectToAction(nameof(Index));
                }
           
            else
            {
                if (noOfImg == 5)
                {
                    ViewBag.ImgUrl = "This Product is already have 5 images!!";
                }

                var model2 = new SecImgViewModel
                {
                    Category = categoryRepository.list().ToList(),
                    PrdId = model.PrdId// should be added to save when refresh after create and no img
                };
                return View(model2);
            }

        }

        public ActionResult GetPhoto(int catId, int prdId)
        {
            if (prdId == 0)
            {
                var secondaryImg = secondaryImgViewRepository.list().Where(s => s.CatId == catId).ToList();
                return PartialView("_ImgSec", secondaryImg);
            }
            else
            {
                var secondaryImg = secondaryImgViewRepository.list().Where(s => s.PrdId == prdId).ToList();
                return PartialView("_ImgSec",secondaryImg);
            }
        }

        // GET: SecondaryImgController/Edit/5
        public ActionResult Edit(int id)
        {
            var secImgView = secondaryImgViewRepository.Find(id);
            SecImgViewModel secImgViewModel = new SecImgViewModel()
            {
                ImgId=secImgView.ImgId,
                PrdId=secImgView.PrdId,
                SecPic=secImgView.SecPic,
                CatId=secImgView.CatId,     
            };
            ViewBag.CurPrd = secImgView.PrdName;
            ViewBag.CurCat = secImgView.CatName;

            return View(secImgViewModel);
        }

        // POST: SecondaryImgController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SecImgViewModel viewModel)
        {
          //if (ModelState.IsValid)
          try
            {

                string fileName = UploadFile(viewModel.File, viewModel.SecPic, viewModel.ImgId);

                SecondaryImg secondaryImg = new SecondaryImg
                {
                    ImgId=viewModel.ImgId,
                    PrdId=viewModel.PrdId,
                    CatId=viewModel.CatId,
                    SecPic=fileName

                };
                secondaryImgRepository.Update(viewModel.ImgId, secondaryImg);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
      
        }

        // GET: SecondaryImgController/Delete/5
        public ActionResult Delete(int id)
        {
            var secImgView = secondaryImgViewRepository.Find(id);
            return View(secImgView);
        }

        // POST: SecondaryImgController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,SecImgViewModel secImgView)
        {
            try
            {
                DeleteImg(secImgView.SecPic,id);
                secondaryImgRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetProducts(int catId)
        {// used in json => Jquery
            var prdByCat = prdByCatRepository.list().Where(p => p.CatId == catId).ToList();

            var prodLists = new List<ProdList> { };
            for (var i = 0; i < prdByCat.Count; i++)
            {
                prodLists.Add(new ProdList { PrdId = prdByCat[i].PrdId, NameId = prdByCat[i].PrdName });
            }

            return Json(prodLists);
        }
        //****************************
        public void DeleteImg(string imgUrl, int id)
        {
            string uploads = Path.Combine(hosting.WebRootPath, "uploads/secondryImg");
            string oldPath = Path.Combine(uploads, imgUrl);
            if (IsImgUploaded(imgUrl, id) == false)
            {
                System.IO.File.Delete(oldPath);
            }
        }
        string UploadFile(IFormFile file)
        {
          if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads/secondryImg");
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
                string uploads = Path.Combine(hosting.WebRootPath, "uploads/secondryImg");
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
            var InSec = secondaryImgViewRepository.list()
                .Where(img => img.SecPic == imageUrl && img.ImgId != trk).ToList();
            if (InSec.Count == 0)
            {
                return false;

            }
            else
            {
                return true;
            }
        }
    }
}
