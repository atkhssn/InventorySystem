using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.BillGenerateds
{
    public class BillGeneratedViewModel:BaseViewModel
    {
        public string BillNo { set; get; }
        public string Email { set; get; }
        public string FullName { set; get; }       
        public int Month { set; get; }
        public int Years { set; get; }
        public string UserId { set; get; }
        public bool IsPaid { set; get; }
        public DateTime BillGeneratedDate { set; get; }
        public decimal SubscriptionFee { get; set; }
        public decimal CollactionFee { get; set; }
        public decimal PayBill { get; set; }
        public decimal TotalFee { get; set; }
        public List<BillGeneratedViewModel> bills { set; get; } 
    }
}
