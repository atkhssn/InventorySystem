using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class SalesReturn:BaseEntity
    {
        public string SalesReturnNo { get; set; }
        public long CustomerId { get; set; }
        public DateTime SalesReturnDate { get; set; }
        public string Reason { get; set; }
        public bool IsSubmited { get; set; }
    }
}
