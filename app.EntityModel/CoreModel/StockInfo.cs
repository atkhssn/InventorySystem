using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class StockInfo:BaseEntity
    {
        public int StockTypeId { set; get; }
        public long ProductId { set; get; }
        public long ReferenceId { set; get; }
        public string ReferenceNo { set; get; }
        public decimal InQty { set; get; }
        public decimal InPrice { set; get; }
        public decimal OutQty { set; get; }
        public decimal OutPrice { set; get; }
        public decimal SaleReturnQty { set; get; }
        public decimal SaleReturnPrice { set; get; }
        public decimal PurchesReturnQty { set; get; }
        public decimal PurchesSaleReturnPrice { set; get; }
        public decimal CogsPrice { set; get; }
        public DateTime ReceivedDate { set; get; }
    }
}
