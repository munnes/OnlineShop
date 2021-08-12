using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Advertisment
    {
        [Key]
        public int AdvId { get; set; }
        public string AdvTitle { get; set; }
        public int PrdId { get; set; }
        public string AdvTxt { get; set; }
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DFrom { get; set; }
        [Display(Name = "To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DTo { get; set; }
    }
}
