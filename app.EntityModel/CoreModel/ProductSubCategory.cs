using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.CoreModel
{
    public partial class ProductSubCategory:BaseEntity
    {
        public string Name { get; set; }
        public long ProductCategoryId { get; set; }
        public string ProductType { get; set; }
        public string Remarks { get; set; }
        public int OrderNo { get; set; }
        public int AccountingHeadId { get; set; }
        public int AccountingIncomeHeadId { get; set; }
        public int AccountingExpenseHeadId { get; set; }
    }
}
