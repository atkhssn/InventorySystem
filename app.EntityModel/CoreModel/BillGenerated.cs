using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public class BillGenerated:BaseEntity
    {
        public string BillNo {set; get; }
        public int Month {set; get; }
        public int Years {set; get; }
        public string UserId {set; get; }
        public bool IsPaid {set; get; }
        public decimal SubscriptionFee {get; set; }
        public decimal CollactionFee { get; set; }
    }
}
