using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Classes;
using OnlineShop.Models.Repositories;
using OnlineShop.Models;
using OnlineShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OnlineShop.Models.Email;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class CouriorController : Controller

    {
        private readonly IOnlineShopRepository<CouriersView> couriorViewRepository;
        private readonly IOnlineShopRepository<UsrTrans> usrTransRepository;
        private readonly IOnlineShopRepository<Purchases> purchasesRepository;
        private readonly IOnlineShopRepository<PurchaseView> purchaseViewRepository;
        private readonly IMailService mailService;
        private static bool sendAll;
        private static bool IsIndex = true;
        public CouriorController(IOnlineShopRepository<CouriersView> couriorViewRepository,
            IOnlineShopRepository<UsrTrans> usrTransRepository,
            IOnlineShopRepository<Purchases> purchasesRepository,
            IOnlineShopRepository<PurchaseView> purchaseViewRepository,
             IMailService mailService)
        {
            this.couriorViewRepository = couriorViewRepository;
            this.usrTransRepository = usrTransRepository;
            this.purchasesRepository = purchasesRepository;
            this.purchaseViewRepository = purchaseViewRepository;
            this.mailService = mailService;
        }
        // GET: CouriorController

        public ActionResult Index()
        {
            IsIndex = true;
            CarryViewModel carryViewModel = new CarryViewModel();
            return View(FillPurchase(carryViewModel));
        }

        [HttpPost]
        public async Task<ActionResult> Index(CarryViewModel carryViewModel)
        {
            List<int> orders = new List<int>();
            List<int> prdId =new List<int>();
            int i = 0;
            ViewBag.All = carryViewModel.Purchases.Count();
            if (sendAll)
            {
                foreach (var carry in carryViewModel.Purchases)
                {
                    if (IsValid(carry.OrdStat, carry.ExpDate.ToString(), carry.CurId))
                    {
                        var purchase = purchasesRepository.Find(carry.TrkOut);
                        purchase.OrdStat = carry.OrdStat;
                        purchase.CurId = carry.CurId;
                        purchase.ExpDate = Convert.ToDateTime(carry.ExpDate).Date;
                        orders.Insert(i, carry.OrdId);
                        prdId.Insert(i, carry.PrdId);
                        i++;
                        purchasesRepository.Update(carry.TrkOut, purchase);
                    }

                }
                List<int> dist = orders.GroupBy(s => s).Select(s => s.First()).ToList();
                for (int j = 0; j < dist.Count(); j++)
                {
                    var usrTrans = usrTransRepository.list()
                            .Where(trn => trn.OrdId == orders.ElementAt(j) && trn.OrdStat == 2).ToList();

                    //*****************************
                    var emailBody = "Hi " + usrTrans.ElementAt(0).Customer + "; <br> Thak you for shoping with Queen, " +
                    "Your below item (s) had been dispatched. " +
                    "please find below items and expexted delivery date <br><br>";
                    emailBody = emailBody + " <table class='table table-bordered' style= 'width:70%'>" +
                    "<thead style = 'background-color: #30aabc;color: white'>" +
                    "<tr><th>Product</th>" +
                    "<th>Quantity</th>" +
                    "<th>Expexted delivery date</th><tr/></thead><tbody>";
                    foreach (var ord in usrTrans)
                    {
                        emailBody = emailBody + "<tr><td>" + ord.PrdName + " </td><td>"
                            + ord.Qty + "</td><td>" + Convert.ToDateTime(ord.ExpDate).ToShortDateString() + "</td></tr>";
                    }
                    emailBody = emailBody + "<tr/></tbody> ";
                    emailBody = emailBody + "<br><br><p> We hope to see you again soon.<br>Queen Team<p>";
                  await SendEmail(emailBody, usrTrans[0].Email);
                    //*****************************
                }
                return RedirectToAction(nameof(SentItems));
            }
            else
            {
                return View(FillPurchase(carryViewModel));
            }
        }

        public CarryViewModel FillPurchase(CarryViewModel carryViewModel)
        {carryViewModel.Purchases = purchasesRepository.list()
            .Where(ord => ord.OrdStat == 1).ToList();
            carryViewModel.PurchaseView = purchaseViewRepository.list()
            .Where(ord => ord.OrdStat == 1).ToList();
            carryViewModel.CouriersView = couriorViewRepository.list().ToList();
            ViewBag.Status = FillStatus();
            return carryViewModel;
        }
        //**********************
        public async Task SendEmail(string emailBody, string email)
        {
            MailRequest mailRequest = new MailRequest();
            //--------------------------
            string body = "<h3>Dispatched items </h3><hr/>" + emailBody;

            mailRequest.Body = body;
            mailRequest.ToEmail = email;
            mailRequest.Subject = "Dispatched items email";
            //**********************
            try
            {
           await mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //*******
        public bool IsValid(int status, string exp, string curId)
        {
            if (status == 2)
            {
                if (curId == null)
                {
                    return false;
                }
                else if (exp.Trim() == null)
                {
                    return false;
                }
                else if (Convert.ToDateTime(exp).Date < DateTime.Now.Date)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else { return false; }

        }

        public void CanSend(bool send)
        {
            sendAll = send;
        }
        public ActionResult Done()
        {
            return View();
        }
        public ActionResult NotDone()
        {
            return View();
        }
        public List<OrdStatus> FillStatus()
        {
            List<OrdStatus> statList = new List<OrdStatus>();
            statList.Add(new OrdStatus() { OrdStat = 1, StatName = "Pending" });
            statList.Add(new OrdStatus() { OrdStat = 2, StatName = "Send" });
            //  statList.Add(new OrdStatus() { OrdStat = 3, StatName = "On The Way" });
            return statList;
        }
        public ActionResult SentItems()
        {
            IsIndex = false;
            var usrTrans = usrTransRepository.list()
                .Where(ord => ord.OrdStat == 2).ToList();
            return View(usrTrans);
        }
        public ActionResult Search(string term)
        {
           // ViewData["Cart"] = QtyCart()[0];
            //ViewBag.Ctrl=
            if (IsIndex)
            {
               CarryViewModel carryViewModel = new CarryViewModel();
              carryViewModel.PurchaseView = purchaseViewRepository.Search(term).Where(ord => ord.OrdStat == 1).ToList();
            
                List<Purchases> purchase = new List<Purchases>();
                 for(int i=0; i< carryViewModel.PurchaseView.Count; i++)
                {
                    purchase.Add(purchasesRepository.Find(carryViewModel.PurchaseView[i].TrkOut));
                }
                carryViewModel.Purchases = purchase.ToList();
                carryViewModel.CouriersView = couriorViewRepository.list().ToList();
                ViewBag.Status = FillStatus();
                return View("Index", carryViewModel);
            }

            else
            {
             var result = usrTransRepository.Search(term).Where(ord => ord.OrdStat == 2).ToList();
                return View("SentItems", result);
            }

        }

    }
}

      

       
