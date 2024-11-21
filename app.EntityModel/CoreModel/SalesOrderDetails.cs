using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class SalesOrderDetails:BaseEntity
    {
        public long SalesOrderId { get; set; }
        public long ProductId { get; set; }
        public string UnitName { get; set; }
        public decimal SalesQty { get; set; }
        public decimal SalesRate { get; set; }
        public decimal SalesAmount {get; set; }
        public decimal Discount_Persentage {get; set; }
        public decimal Discount_Amount {get; set; }
        public string PackSize { get; set; }
    }
}
