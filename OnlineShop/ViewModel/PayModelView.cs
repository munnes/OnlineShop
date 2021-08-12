using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class PayModelView
    {
        public string Token { get; set; }
        public double Total { get; set; }
        public string Email { get; set; }
        public int PrdId { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public bool IsHome { get; set; }
        public string PrdName { get; set; }
    }
}
