using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
   
    public class ReviewsController : Controller
    {
        private readonly IOnlineShopRepository<UsrTrans> usrTransRepository;
        private readonly IOnlineShopRepository<Reviews> reviewsRepository;
        private readonly IOnlineShopRepository<Purchases> purchaseRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public static string uID;
        public ReviewsController(IOnlineShopRepository<UsrTrans> usrTransRepository,
            IOnlineShopRepository<Reviews> reviewsRepository,
            IOnlineShopRepository<Purchases> purchaseRepository, 
            UserManager<ApplicationUser> userManager
        )
        {
            this.usrTransRepository = usrTransRepository;
            this.reviewsRepository = reviewsRepository;
            this.purchaseRepository = purchaseRepository;
            this.userManager = userManager;
        }
        // GET: ReviewsController
    

        // GET: ReviewsController/Create
        [Authorize]
        public ActionResult Create(int trkOut)
        {
            uID = userManager.GetUserAsync(User).Result.Id;
            Cart();
            var usrTran = usrTransRepository.Find(trkOut);
            if (usrTran.UsrId == uID)
            {
                ViewBag.Prd = usrTran.PrdName;
                ViewBag.Pic = usrTran.Pic;
                ViewBag.Desc = usrTran.Description;
                ViewBag.PrdId = usrTran.PrdId;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public void Cart()
        {
            var purchase = purchaseRepository.list()
                      .Where(pr => pr.IsCart == 1 && pr.UsrId ==uID ).ToList();
            var lstQty = from q in purchase select q.Qty;
            ViewData["Cart"] = lstQty.Sum();
        }

        // POST: ReviewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reviews review)
        {
            try
            {
                review.RevDate = DateTime.Now;
                review.UsrId = uID;
                reviewsRepository.Add(review);


                return RedirectToAction(nameof(Thanks4Review));
            }
            catch
            {
                return View();
            }
        }
        [Authorize]
        public ActionResult Thanks4Review()
        {
            Cart();
            return View();
        }

        // GET: ReviewsController/Edit/5
   

        // GET: ReviewsController/Delete/5
      
    }
}
