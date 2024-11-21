using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class PurchaseReturn:BaseEntity
    {
        public string PurchaseReturnNo { get; set; }
        public long SupplierId { get; set; }
        public DateTime PurchaseReturnDate { get; set; }
        public string Reason { get; set; }
        public bool IsSubmited { get; set; }
    }
}
