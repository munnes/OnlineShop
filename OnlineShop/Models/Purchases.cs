using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Purchases
    {
        [Key]
        public int TrkOut { get; set; }
        public int PrdId { get; set; }
        public int Qty { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double Price { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ODate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExpDate { get; set; }
        public bool InCart { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int IsCart { get; set; }
        public bool IsChkOut { get; set; }
        public string UsrId { get; set; }
        public int OrdId { get; set; }
        public bool ActvOrd { get; set; }
        public int OrdStat { get; set; }
        public string CurId { get; set; }
        public DateTime? RecDate { get; set; }


    }
}
