using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.PurchaseOrder_Services
{
    public class PurchaseOrderViewModel:BaseViewModel
    {
        public string PurchaseOrderNo { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierMobile { get; set; }
        public string SupplierAddress { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string TermsAndCondition { get; set; }
        public string Description { get; set; }
        public string SupplierPaymentMethodEnumFK { get; set; }
        public decimal BankCharg { get; set; }
        public decimal TransportCharges { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal Amount { get; set; }
        public bool IsCancel { get; set; }
        public bool IsHold { get; set; }
        public bool IsSubmited { get; set; }
        public int Status { get; set; }
        public IEnumerable<PurchaseOrderDetailsViewModel> MappVm { get; set; }
        public IEnumerable<PurchaseOrderViewModel> datalist { get; set; }
    }
}
