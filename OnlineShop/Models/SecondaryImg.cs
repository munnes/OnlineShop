using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class SecondaryImg
    {
        [Key]
        public int ImgId { get; set; }

        public int PrdId { get; set; }
        public string SecPic { get; set; }

        public int CatId { get; set; }
    }
}
