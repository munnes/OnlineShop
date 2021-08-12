using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class AvailStock
    {
        public int LastTrkIn { get; private set; }
        public int PrdId { get; private set; }
        [Display(Name = "Item Code")]
        public string PrdCode { get; private set; }

        [Display(Name = "Product")]
        public string PrdName { get; private set; }
        public int CatId { get; private set; }

        [Display(Name = "Category")]
        public string CatName { get; private set; }
        [Display(Name = "Quantity")]
        public int AvlQty { get; private set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double Price { get; private set; }// double map to float in DB

        [Display(Name = "Product Picture")]
        public string Pic { get; private set; }
        public string NameId { get; private set; }
        public string Description { get; private set; }

        public string MainDesc { get; private set; }
    }
}
