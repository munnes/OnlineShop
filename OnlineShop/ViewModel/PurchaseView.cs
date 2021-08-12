using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class PurchaseView
    {
        public int TrkOut { get; private set; }
        public int PrdId { get; private set; }
        public int OrdStat { get; set; }
        public string NameId { get; private set; }
        public string Pic { get; private set; }
    }
}
