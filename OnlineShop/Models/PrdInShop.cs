using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public partial class PrdInShop
    {
        [Key]
        public int Trk { get; set; }
        public int PrdId { get; set; }
        public int Qty { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double Price { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateAdd { get; set; }
        public string Pic { get; set; }

    }
}
