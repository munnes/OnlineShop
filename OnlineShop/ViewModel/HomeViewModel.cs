using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<AvailStock> AvailStock { get; set; }
        public IEnumerable<AdvertismentView> AdvertismentView { get; set; }
    }

}
