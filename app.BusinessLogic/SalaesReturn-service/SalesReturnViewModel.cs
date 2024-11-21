using app.Services.PurchaseOrder_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.SalaesReturn_service
{
    public class SalesReturnViewModel:BaseViewModel
    {
        public string SalesReturnNo { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime SalesReturnDate { get; set; }
        public string Reason { get; set; }
        public bool IsSubmited { get; set; }
        public List<SalesReturnDetailsViewmodel> MappVm { get; set; }
        public IEnumerable<SalesReturnViewModel> datalist { get; set; }
    }
}
