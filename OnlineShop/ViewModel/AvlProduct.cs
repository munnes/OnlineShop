using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class AvlProduct
    {
        public int PrdId { get; private set; }
        [Required]
        [Display(Name = "Item Code")]
        public string PrdCode { get; private set; }
        [Required]
        [Display(Name = "Item Name")]
        public string PrdName { get; private set; }
        public string NameId { get; private set; }
        [Required]
        public int CatId { get; private set; }
    }
}
