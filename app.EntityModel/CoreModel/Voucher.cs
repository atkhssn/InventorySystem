using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class Voucher:BaseEntity
    {
     public int VoucherTypeId { set; get; }
     public string VoucherNo { set; get; }
     public string PaymentMethod { set; get; }
     public DateTime VoucherDate { set; get; }
     public string Narration { set; get; }
     public long ReferenceId { set; get; }
     public long VendorId { set; get; }
     public bool IsSubmitted { set; get; }
    }
}
