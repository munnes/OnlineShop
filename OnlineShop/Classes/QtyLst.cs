using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Classes
{
    public class QtyLst
    {
        public string qtyTxt { get; set; }
        public int totalQty { get; set; }
        public int totalQtyActv { get; set; }
        public bool avl { get; set; }
        public int usrQty { get; set; }
        public double subTotal { get; set; }
    }
}
