using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Voucher_Service
{
    public class VoucherViewModel:BaseViewModel
    {
        public int VoucherTypeId { set; get; }
        public int SupplierPaymentMethodEnumFK { set; get; }
        public string PaymentMethod { set; get; }
        public string VoucherTypeCode { set; get; }
        public string VoucherNo { set; get; }
        public DateTime VoucherDate { set; get; }
        public string Narration { set; get; }
        public long ReferenceId { set; get; }
        public string ReferenceNo { set; get; }
        public long VendorId { set; get; }
        public string VendorName { set; get; }
        public string VendorMobile { set; get; }
        public string VendorEmail { set; get; }
        public string VendorAddress { set; get; }
        public decimal Amount { set; get; }
        public decimal DebitAmount { set; get; }
        public decimal CreditAmount { set; get; }
        public decimal ReturnAmount { set; get; }
        public decimal Blance { set; get; }
        public List<VoucherDetalisViewModel> voucherDetalisViewModels { set; get; }
        //public IEnumerable<VoucherDetalisViewModel> ModelVM { set; get; }
        public IEnumerable<VoucherViewModel> voucherlist { set; get; }
        public List<VoucherViewModel> datalist { set; get; }
    }
}
