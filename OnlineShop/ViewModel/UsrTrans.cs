using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.ViewModel
{
    public class UsrTrans
    {
        public int TrkOut { get; private set; }
        public int PrdId { get; private set; }
        public int Qty { get; private set; }
        [DisplayFormat(DataFormatString = "{0:n2}")]
        public double Price { get; private set; }
        [Display(Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ODate { get; private set; }

        public bool InCart { get; private set; }
       // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int IsCart { get; private set; }
        public bool IsChkOut { get; private set; }
        public string UsrId { get; private set; }
        public string Pic { get; private set; }
        public string PrdName { get; private set; }
        public string PrdCode { get; private set; }
        public string Description { get; private set; }
        public string Email { get; private set; }
        public bool ActvOrd { get; private set; }
        public  string NameId { get; private set; }
        public int OrdId { get; private set; }
        public int OrdStat { get; private set; }
        public string CurId { get; private set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ExpDate { get; private set; }
        public string Customer { get; private set; }
        public string CourName { get; private set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? RecDate { get; private set; }
        public string CustomerAddress { get; private set; }



    }
}
