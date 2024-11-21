using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class VoucherDetails:BaseEntity
    {
        public long VoucherId { set; get; }
        public long ProductId { set; get; }
        public decimal DebitAmount { set; get; }
        public decimal CreditAmount { set; get; }
        public long ReferenceId { set; get; }
        public string Titel { set; get; }
        public string Particular { set; get; }
    }
}
