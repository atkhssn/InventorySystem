using app.EntityModel.DatabaseView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Report_service
{
    public class ReportsViewModel
    {
        public long? CustomerId { set; get; }
        public long? SupplierId { set; get; }
        public long? CompanyId { set; get; }
        public long? ProductId { set; get; }
        public string VoucherCode { set; get; }
        public string TrakingId { get; set; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public List<SalesViewReport> sales { set; get; }
    }
}
