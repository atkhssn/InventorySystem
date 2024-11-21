using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class SalesOrder:BaseEntity
    {
        public string SalesOrderNo { get; set; }
        public long CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAddress { get; set; }
        public string TermsAndCondition { get; set; }
        public string Description { get; set; }
        public string SupplierPaymentMethodEnumFK { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal Discountpercentages { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsCancel { get; set; }
        public bool IsSubmited { get; set; }
        public int Status { get; set; }
    }
}
