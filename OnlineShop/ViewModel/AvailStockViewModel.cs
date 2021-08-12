using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class AvailStockViewModel
    {
        public IEnumerable<AvailStock> AvailStock { get; set; }
        public IEnumerable<ReviewsView> ReviewsView { get; set; }
        public IEnumerable<SecondaryImg> SecondaryImg { get; set; }
    }
}
