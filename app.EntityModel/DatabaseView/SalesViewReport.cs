using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.DatabaseView
{
    public class SalesViewReport
    {
        [Key]
        public int Serialno { get; set; }
        public long ProductId { set; get; }
        public string SalesOrderNo { set; get; }
        public long CustomerId { set; get; }
        public DateTime SalesDate { set; get; }
        public long CompanyId { set; get; }
        public string CompanyName { set; get; }
        public string CoustomerName {set; get; }

        public string CoustomerMobile { set; get; }     
        public string ProductName { set; get; }
        public string ProductSubCategoryName { set; get; }
        public string ProductCategoryName { set; get; }
        public decimal SalesQty { set; get; }
        public decimal SalesAmount { set; get; }

        public decimal OtherCharge { set; get; }
    }
}
