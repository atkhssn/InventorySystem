using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public partial class Product:BaseEntity
    {
        public string ProductType { get; set; }
        public long ProductCategoryId { get; set; }
        public long ProductSubCategoryId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ShortName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal CreditSalePrice { get; set; }
        public decimal SaleCommissionRate { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal PurchaseCommissionRate { get; set; }
        public decimal TPrice { get; set; }
        public decimal CostingPrice { get; set; }
        public int CompanyId { get; set; }
        public string UnitName { get; set; }
    }
}
