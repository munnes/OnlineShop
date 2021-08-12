using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Models;

namespace OnlineShop.ViewModel
{
    public class PrdByCat
    {
        public int PrdId { get; private set; }
        [Display(Name = "Item Code")]
        public string PrdCode { get; private set; }

        [Display(Name = "Item Name")]
        public string PrdName { get; private set; }
        public int CatId { get; private set; }
        [Display(Name = "Category")]
        public string CatName { get; private set; }
        public string NameId { get; private set; }
        [DataType(DataType.MultilineText)]
   
        public string Description { get; private set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string MainDesc { get; private set; }


    }
}
