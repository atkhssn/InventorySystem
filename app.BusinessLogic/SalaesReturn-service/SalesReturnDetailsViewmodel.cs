using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.SalaesReturn_service
{
    public class SalesReturnDetailsViewmodel:BaseViewModel
    {
        public long SalesReturnId { set; get; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal ReturnQty { get; set; }
        public decimal ReturnRate { get; set; }
        public decimal ReturnAmount { get; set; }
    }
}
