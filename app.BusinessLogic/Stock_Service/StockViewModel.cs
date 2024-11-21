using app.Services.Product_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Stock_Service
{
    public class StockViewModel:BaseViewModel
    {
        public int StockTypeId { set; get; }
        public long ProductId { set; get; }
        public long CompanyId { set; get; }
        public string CompanyName { set; get; }
        public string ProductName { set; get; }

        public string ProductSubCategoryName { get; set; }
        public long ProductSubCategoryId { get; set; }
        public long ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }

        public long ReferenceId { set; get; }
        public string ReferenceNo { set; get; }
        public decimal InQty { set; get; }
        public decimal InPrice { set; get; }
        public decimal OutQty { set; get; }
        public decimal OutPrice { set; get; }
        public decimal CogsPrice { set; get; }
        public DateTime ReceivedDate { set; get; }
        public IEnumerable<StockViewModel> StockList { get; set; }
    }
}
