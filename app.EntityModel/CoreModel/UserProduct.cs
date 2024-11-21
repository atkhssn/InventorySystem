using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class UserProduct:BaseEntity
    {
        public long ProductId { set; get; }
        public long CompanyId { set; get; }
        public decimal UnitPrice {set; get; }
        public decimal AVGPrice { set; get; }
        public decimal MRP { set; get; }
    }
}
