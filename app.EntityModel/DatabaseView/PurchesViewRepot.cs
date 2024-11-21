using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.DatabaseView
{
    public class PurchesViewRepot
    {
        [Key]     
        public int Serialno { get; set; }
        public long PurchaseId { set; get; }
        public string PurchaseNo { set; get; }
        public string SupplierName { set; get; }
        public string SupplierMobile { set; get; }
        public string CompanyName { set; get; }
        public string ProductName { set; get; }
        public string ProductSubCategoryName { set; get; }
        public string ProductCategoryName { set; get; }
        public decimal CogsPrice { set; get; }
        public decimal InQty { set; get; }
        public decimal InPrice { set; get; }
        public long SupplierId { set; get; }
        public long ProductId { set; get; }
        public DateTime PurchaseDate { set; get; }
    }
}
