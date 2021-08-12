using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Classes;
using Stripe;
using OnlineShop.Models.Email;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Controllers
{
  public class HomeController : Controller
    {
      
        private readonly IOnlineShopRepository<AvailStock> availStockRepository;
        private readonly IOnlineShopRepository<Purchases> purchasesRepository;
        private readonly IOnlineShopRepository<PrchFRecView> prchFRecViewRepository;
        private readonly IOnlineShopRepository<UsrTrans> usrTransRepository;
        private readonly IOnlineShopRepository<Orders> ordersRepository;
        private readonly IOnlineShopRepository<AdvertismentView> advertismentViewRepository;
        private readonly IOnlineShopRepository<ReviewsView> reviewsViewRepository;
        private readonly IOnlineShopRepository<SecondaryImg> secondaryImgRepository;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMailService mailService;
        //  private static bool IsIndex=true ;
        public static int SearchFactor = 1;
        public static string uID;
        public static string CName="";


        public HomeController(IOnlineShopRepository<AvailStock> availStockRepository,
            IOnlineShopRepository<Purchases> purchasesRepository,
            IOnlineShopRepository<PrchFRecView> prchFRecViewRepository,
            IOnlineShopRepository<UsrTrans> usrTransRepository,
            IOnlineShopRepository<Orders> ordersRepository,
            IOnlineShopRepository<AdvertismentView> advertismentViewRepository,
            IOnlineShopRepository<ReviewsView> reviewsViewRepository,
            IOnlineShopRepository<SecondaryImg> secondaryImgRepository,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMailService mailService


            )
        {
            this.availStockRepository = availStockRepository;
            this.purchasesRepository = purchasesRepository;
            this.prchFRecViewRepository = prchFRecViewRepository;
            this.usrTransRepository = usrTransRepository;
            this.ordersRepository = ordersRepository;
            this.advertismentViewRepository = advertismentViewRepository;
            this.reviewsViewRepository = reviewsViewRepository;
            this.secondaryImgRepository = secondaryImgRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mailService = mailService;
        }

      
      [HttpPost]
        public async Task<ActionResult> Charge(PayModelView Data)

        {
            var custName = userManager.GetUserAsync(User).Result.FullName;
            var emailBody = "Hi " + custName + "; <br> Thak you for shoping with Queen, " +
                "We will inform you once your item (s) has been dispatched. " +
                "please find below your bill details <br><br>";
            emailBody = emailBody + " <table class='table table-bordered' style= 'width:70%'>" +
          "<thead style = 'background-color: #30aabc;color: white'>" +
            "<tr><th>Product</th>" +
            "<th>Quantity</th>" +
            "<th>Price</th><tr/></thead><tbody><tr><td>";

            ViewData["Cart"] = QtyCart()[0];
            // StripeConfiguration.SetApiKey="Publishablekey";
            // StripeConfiguration.ApiKey="Secretkey";
            var customers = new CustomerService();
            var charges = new ChargeService();
            var usrId = userManager.GetUserAsync(User).Result.Id;
            var total = Convert.ToDouble(Data.Total.ToString());
            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = Data.Email,
                Source = Data.Token,

            });


            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = (long?)total * 100,
                Description = "Confirm Order email",
                Currency = "AED",
                Customer = customer.Id,
                ReceiptEmail = Data.Email,
                Metadata = new Dictionary<string, string>()
                {
                    {"OrderId","111" },
                    {"PostCode","LeTTy88" }
                }

            });
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                Orders order = new Orders
                {
                    UsrId = usrId,
                    Total = total,
                    ordDate = DateTime.Now

                };
                ordersRepository.Add(order);
                var allOrd = ordersRepository.list().Where(o => o.Total == total && o.UsrId == usrId);
                var ord = allOrd.Max(o => o.OrdId);
                if (Data.IsHome)
                {
                    CreatePurchase(Data.PrdId, Data.Qty, Data.Price, false, true, usrId, ord, false,1);
                    emailBody = emailBody + Data.PrdName + "</td><td>" + Data.Qty + "</td><td>" + Data.Price + " AED</td></tr>";

                }
                else
                {/*update*/
                    var acvToOrder = usrTransRepository.list().Where(o => o.UsrId == usrId && o.ActvOrd == true).ToList();
                    foreach (var pr in acvToOrder)
                    {
                        var purchase = purchasesRepository.list().SingleOrDefault(o => o.TrkOut == pr.TrkOut);
                        purchase.InCart = false;
                        purchase.IsChkOut = true;
                        purchase.ActvOrd = false;
                        purchase.OrdStat = 1;
                        purchase.OrdId = ord;
                     
                        emailBody = emailBody + pr.PrdName + "</td><td>" + pr.Qty + "</td><td>AED " + pr.Price + "</td></tr><tr><td>";
                        purchasesRepository.Update(pr.TrkOut, purchase);

                    }

                }
                emailBody = emailBody + "<tr><td></td><td></td><td style='color:red;'>Subtotal=  AED " + total + "</td><tr/></tbody>";
                emailBody = emailBody + "<br><br><p> We hope to see you again soon.<br>Queen Team<p>";
            await SendEmail(emailBody, ord);
                ViewData["Cart"] = QtyCart()[0];
                return View();
            }

            return View();
        }
        //**********************
        public async Task SendEmail(string emailBody, int ord)
        {
            MailRequest mailRequest = new MailRequest();
    
            var UserEmail = userManager.GetUserAsync(User).Result.Email;
            //--------------------------
            string body = "<h3>Confirm Order email #" + ord + "</h3><hr/>" + emailBody;
            
            // mailRequest.Body = body + "<img src='https://www.countryflags.io/be/flat/64.png'>";  
            mailRequest.Body = body;
            mailRequest.ToEmail = UserEmail;
            mailRequest.Subject = "Confirm Order email";
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

        //****
        /*done*/
        [Authorize]
        public ActionResult ChkOut(int prdId, int qty, double price, bool isHome, string prdName = "")
        {
            ViewData["Cart"] = QtyCart()[0];
            if (isHome)
            {
                ViewBag.PrdId = prdId;
                ViewBag.Total = Convert.ToDouble(price * qty);
            }
            else
            {
                ViewBag.Total = Convert.ToDouble(price);
            }
            ViewBag.Price = Convert.ToDouble(price);
            ViewBag.Qty = Convert.ToDouble(qty);
            ViewBag.Email = userManager.GetUserAsync(User).Result.Email;
            ViewBag.IsHome = isHome;
            ViewBag.PrdName = prdName;
            string loc = userManager.FindByIdAsync(uID).Result.Location;
            string phone = userManager.FindByIdAsync(uID).Result.PhoneNumber;
            if (loc == null || phone == null)
            {
                if (phone == null)
                { TempData["Phone"] = "Your Phone number is mandatory to proceed your checkout "; }
                if (loc == null)
                { TempData["Loc"] = "Your Location is mandatory to proceed your checkout "; }
                return RedirectToAction("Index", "Identity/Account/Manage");
            }
            return View();
        }
        //*****************
        /*updated*/
        public void ActivatePurchase(bool actvOrd, int trkOut)

        {
            var usrId = userManager.GetUserAsync(User).Result.Id;
            var purchase = purchasesRepository.list().SingleOrDefault(pr => pr.TrkOut == trkOut);
            purchase.ActvOrd = actvOrd;

            purchasesRepository.Update(trkOut, purchase);
        }

        //*********************
     
        public ActionResult Index()
        {
            if (signInManager.IsSignedIn(User)) { 
            uID = userManager.GetUserAsync(User).Result.Id;
            }
            SearchFactor = 1;
            ViewData["Admin"] = HttpContext.User.IsInRole("Administrators");
            ViewData["Cart"] = QtyCart()[0];
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.AvailStock = availStockRepository.list().ToList();
            homeViewModel.AdvertismentView = advertismentViewRepository.list()
                .ToList().Where(t => t.DFrom <= DateTime.Now && t.DTo >= DateTime.Now);
            return View(homeViewModel);
        }
       
        public ActionResult IndexByCategory(string catName = "")
        {
            SearchFactor = 2;
            if (catName == "")
            {
                catName = CName;
            }
            else
            {
                CName = catName;
            }
            ViewData["Cart"] = QtyCart()[0];
            AvailStockViewModel availStockViewModel = new AvailStockViewModel();
            availStockViewModel.AvailStock= availStockRepository.list().Where(c => c.CatName == catName).ToList();
            availStockViewModel.ReviewsView = reviewsViewRepository.list();
            availStockViewModel.SecondaryImg = secondaryImgRepository.list();
            return View(availStockViewModel);
        }

      public ActionResult SinglePrd(int prdId)
        
        {
          
            ViewData["Cart"] = QtyCart()[0];
            AvailStockViewModel availStockViewModel = new AvailStockViewModel();
            availStockViewModel.AvailStock = availStockRepository.list().Where(prd => prd.PrdId == prdId).ToList();
            availStockViewModel.ReviewsView = reviewsViewRepository.list().Where(prd => prd.PrdId == prdId).ToList();
            availStockViewModel.SecondaryImg=secondaryImgRepository.list().Where(prd => prd.PrdId == prdId).ToList();
            return View(availStockViewModel);
               }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            ViewData["Cart"] = QtyCart()[0];
             var usrTrans = usrTransRepository.list().Where(trn => trn.IsCart == 1 && trn.UsrId == uID).ToList();
             return View(usrTrans);
        }

        // [HttpPost]
        public ActionResult Delete(int id)
        {
            purchasesRepository.Delete(id);
        
            return RedirectToAction(nameof(ShoppingCart));
        }
      
        [HttpPost]
        public void PurchaseOrder(int prdId, int qty, double price, bool inCart, bool isChkOut,  bool isHome, bool actvOrd = true)
        {
      
            var prchFRecView = FindRec(uID, prdId);

            if (prchFRecView == null)
            {
                CreatePurchase(prdId, qty, price, inCart, isChkOut, uID, 0, actvOrd);
            }
            else
            {
                if (isHome)
                {
                    qty = prchFRecView.Qty + qty;
                    qty = qty <= 10 ? qty : 10;

                }
                EditPurchase(prchFRecView.TrkOut, prdId, qty, price, inCart, isChkOut, uID, actvOrd);

            }
        }

        public PrchFRecView FindRec(string usrId, int prdId)
        {
            var purchase = prchFRecViewRepository.list()
                .SingleOrDefault(pr => pr.IsCart == 1
                && pr.PrdId == prdId
                && pr.UsrId == usrId);
            return purchase;

        }

        public void CreatePurchase(int prdId, int qty, double price, bool inCart, bool isChkOut, string usrId, int ordId = 0, bool actvOrd = true ,int ordStat=0)
        {

            Purchases purchases = new Purchases
            {
                PrdId = prdId,
                Qty = qty,
                Price = price,
                ODate = DateTime.Now,
                InCart = inCart,
                IsChkOut = isChkOut,
                UsrId = usrId,
                OrdId = ordId,
                ActvOrd = actvOrd,
                OrdStat=ordStat,

            };
            purchasesRepository.Add(purchases);
        }


        public void EditPurchase(int trkOut, int prdId, int qty, double price, bool inCart, bool isChkOut, string usrId, bool actvOrd = true)
        {
            Purchases purchases = new Purchases
            {
                TrkOut = trkOut,
                PrdId = prdId,
                Qty = qty,
                Price = price,
                ODate = DateTime.Now,
                InCart = inCart,
                IsChkOut = isChkOut,
                UsrId = usrId,
                ActvOrd = actvOrd,
                
            };
            purchasesRepository.Update(trkOut, purchases);
        }

        public double[] QtyCart()
        {
            var usrId = "";
            if (signInManager.IsSignedIn(User))
            {
                usrId = userManager.GetUserAsync(User).Result.Id;
            }
            var purchase = prchFRecViewRepository.list()
              .Where(pr => pr.IsCart == 1 && pr.UsrId == usrId).ToList();
            var lstQty = from q in purchase select q.Qty;
            var lstQtyActv = (from q in purchase where q.ActvOrd == true select q.Qty);
            var lstTotalActv = from q in purchase where q.ActvOrd == true select q.Qty * q.Price;
            double[] Qty = { lstQty.Sum(), lstQtyActv.Sum(), lstTotalActv.Sum() };

            return Qty;

        }



        public JsonResult GetAvlQty(int prdId, int qty)
        {
            var avlPrd = availStockRepository.Find(prdId);
            string qtyStat = "";
            bool Avl = false;
            if (avlPrd.AvlQty < qty)
            {
                qtyStat = avlPrd.AvlQty > 0 ? "Sorry!! Number of requested product is not Available"
                    : "Sorry!! Product is out of stock";
                Avl = false;
            }
            else
            {
                qtyStat = "";
                Avl = true;
            }

            int total = (int)QtyCart()[0];
            var myLst = new List<QtyLst> { };
            myLst.Add(new QtyLst { qtyTxt = qtyStat, totalQty = total, avl = Avl });

            return Json(myLst);
        }

        //************************
        public JsonResult GetAvlQtyCart(int prdId, string usrId, int newQty)
        {
            var avlPrd = availStockRepository.Find(prdId);
            var usrQty = purchasesRepository.list()
                  .SingleOrDefault(q => q.UsrId == usrId
                  && q.PrdId == prdId
                  && q.IsCart == 1);

            string qtyStat = "";
            int myQty = avlPrd.AvlQty + usrQty.Qty;
            bool Avl = false;
            if (myQty < newQty)
            {
                qtyStat = "Number of requested product is not Available";
                Avl = false;
            }
            else
            {
                qtyStat = "";
                Avl = true;
            }
            int totalCart = (int)QtyCart()[0] - usrQty.Qty + newQty;
            int totalActive = (int)QtyCart()[1] - usrQty.Qty + newQty;
            double subTotal = QtyCart()[2];
            var myLst = new List<QtyLst> { };
            myLst.Add(new QtyLst { qtyTxt = qtyStat, totalQty = totalCart, totalQtyActv = totalActive, avl = Avl, usrQty = usrQty.Qty, subTotal = subTotal });

            return Json(myLst);
        }
        public JsonResult GetInit()
        {
            var myLst = new List<InitCart> { };
            myLst.Add(new InitCart { Total = (int)QtyCart()[1], TotalAmount = QtyCart()[2] });
            return Json(myLst);
        }
        public ActionResult RenderCart()
        {
            return PartialView("_CartPartial");
        }

        public ActionResult Search(string term)
        {
            ViewData["Cart"] = QtyCart()[0];
            //ViewBag.Ctrl=
            switch(SearchFactor)
            {
                case 1:
                HomeViewModel homeViewModel = new HomeViewModel();
                homeViewModel.AdvertismentView = advertismentViewRepository.list().ToList();
                
                homeViewModel.AvailStock = availStockRepository.Search(term);
                // var result = homeViewModel.AvailStock;
                return View("Index", homeViewModel);
                   
                //**********
                case 2:
                AvailStockViewModel availStockViewModel = new AvailStockViewModel();
                availStockViewModel.AvailStock= availStockRepository.Search(term).Where(c=>c.CatName== CName).ToList();
                availStockViewModel.SecondaryImg = secondaryImgRepository.list();
                availStockViewModel.ReviewsView = reviewsViewRepository.list();
               // var result = availStockRepository.Search(term);
                return View("IndexByCategory", availStockViewModel);
                case 3:
                    var result = usrTransRepository.Search(term)
                       .Where(ord => ord.UsrId == uID && (ord.OrdStat == 1 || ord.OrdStat == 2))
                                 .OrderByDescending(ord => ord.TrkOut).ToList();
                return View("UsrOrders",result);

                case 4:
                    var resultHistory = usrTransRepository.Search(term)
                        .Where(ord => ord.UsrId == uID && ord.OrdStat == 3)
                        .OrderByDescending(ord => ord.TrkOut).ToList();

                    return View("UsrHistory", resultHistory);
                default:
                   
                    return RedirectToAction(nameof(Index));
            }

        }
    
        public  ActionResult UsrOrders()
        {
           // uID = usrId;
            SearchFactor = 3;
            ViewData["Cart"] = QtyCart()[0];
                var usrTrans = usrTransRepository.list().Where(ord => ord.UsrId == uID && (ord.OrdStat==1 || ord.OrdStat==2))
                .OrderByDescending(ord => ord.TrkOut).ToList();
                return View(usrTrans);
        }
        [Authorize]
        public ActionResult UsrHistory()
        {
           // uID = usrId;
            SearchFactor = 4;
            ViewData["Cart"] = QtyCart()[0];
            var usrTrans = usrTransRepository.list().Where(ord => ord.UsrId == uID && ord.OrdStat == 3)
            .OrderByDescending(ord => ord.TrkOut).ToList();
            return View(usrTrans);
        }
        [Authorize]
        public ActionResult Receipt(int trkOut)
        {
            ViewData["Cart"] = QtyCart()[0];
            var usrTrans = usrTransRepository.Find(trkOut);
            if (usrTrans.UsrId == uID)
            {
                return View(usrTrans);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult About()
        {
            ViewData["Cart"] = QtyCart()[0];
            return View();
        }
        [Authorize(Roles = "Administrators")]
        public async Task<ActionResult> RegisterViewModel()
        {
            //ViewData["Cart"] = QtyCart()[0];
            List<RegisterViewModel> registerViewModels = new List<RegisterViewModel>() ;
     
            foreach( var user in userManager.Users.ToList())
            { 
                bool admin = (bool)await userManager.IsInRoleAsync(user, "Administrators");
                bool cur = (bool)await userManager.IsInRoleAsync(user, "Couriers");
                var uRole = "";
                if (admin ||cur)
                {

                    if (admin)
                    { uRole= "Administrators"; }
                    else
                    {
                        uRole = "Couriers";
                    }
                   
                    registerViewModels.Add(new RegisterViewModel()
                    {
                        FullName= user.FullName,
                        Email= user.Email,
                        PhoneNumber=user.PhoneNumber,
                        Location=user.Location,
                        Role=uRole

                    });
              }
            }
            return View(registerViewModels);
        }

       
    }
   
}



