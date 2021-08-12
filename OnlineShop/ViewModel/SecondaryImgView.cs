using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class SecondaryImgView
    {
        public int ImgId { get; private set; }

        public int PrdId { get; private set; }
        public string SecPic { get; private set; }

        public int CatId { get; private set; }
        public string CatName { get; private set; }
        public string PrdName { get;private set; }
    }
}
