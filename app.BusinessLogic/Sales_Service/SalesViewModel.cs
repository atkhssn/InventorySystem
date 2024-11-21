using app.Services.PurchaseOrder_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Sales_Service
{
    public class SalesViewModel:BaseViewModel           
    {
        public string SalesOrderNo { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }       
        public string CustomerMobile { get; set; }       
        public string CustomerAddress { get; set; }       
        public string CustomerEmail { get; set; }       
        public DateTime SalesDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string TermsAndCondition { get; set; }
        public string Description { get; set; }
        public string SupplierPaymentMethodEnumFK { get; set; }
        public decimal OtherCharge { get; set; }
        public bool IsCancel { get; set; }
        public bool IsSubmited { get; set; }
        public int Status { get; set; }
        public decimal? Discountpercentages { get; set; }
        public decimal? DiscountAmount { get; set; }
        public List<SalesOrderDetailsViewModel> salesOrderDetailsViews { get;set; }
        public List<SalesOrderDetailsViewModel> MappVm { get;set; }
        public IEnumerable<SalesViewModel> datalist { get; set; }
    }
}
