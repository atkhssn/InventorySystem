using app.Services.SalaesReturn_service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Purchase_Return_Service
{
    public class PurchaseReturnViewModel:BaseViewModel
    {
        public long Id { get; set; }    
        public string PurchaseReturnNo { get; set; }
        public long SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseReturnDate { get; set; }
        public string Reason { get; set; }
        public bool IsSubmited { get; set; }
        public IEnumerable<PurchaseReturnViewModel> datalist { get; set; }
        public  List<PurchaseReturnDetailsViewModel> MappVm { get; set; }
        public string SupplierMobile { get;  set; }
        public string SupplierEmail { get;  set; }
        public string SupplierAddress { get;  set; }
    }
}
