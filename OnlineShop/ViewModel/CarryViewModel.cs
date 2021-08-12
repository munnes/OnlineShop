using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class CarryViewModel
    {
      
        public List<PurchaseView> PurchaseView { get; set; }
        public List<Purchases> Purchases { get; set; }
        public IEnumerable<CouriersView> CouriersView { get; set; }
    }
}
