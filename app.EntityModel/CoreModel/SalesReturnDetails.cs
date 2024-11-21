using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class SalesReturnDetails:BaseEntity
    {
        public long SalesReturnId { set; get; }
        public long ProductId { get; set; }
        public string UnitName { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal ReturnRate { get; set; }
        public decimal ReturnAmount { get; set; }

    }
}
