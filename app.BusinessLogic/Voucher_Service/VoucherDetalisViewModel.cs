using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Voucher_Service
{
    public class VoucherDetalisViewModel:BaseViewModel
    {
        public long VoucherId { set; get; }
        public long ProductId { set; get; }
        public long ProductName { set; get; }
        public decimal DebitAmount { set; get; }
        public decimal CreditAmount { set; get; }
        public long ReferenceId { set; get; }
        public string ReferenceName { set; get; }
        public string Titel { set; get; }
        public string Particular { set; get; }
    }
}
