using app.Services.ProductCategory_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.ProductSubCategory_Service
{
    public class ProductSubCategoryViewModel:BaseViewModel
    {
        public string Name { get; set; }
        public long ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductType { get; set; }
        public string Remarks { get; set; }
        public int OrderNo { get; set; }
        public int AccountingHeadId { get; set; }
        public int AccountingIncomeHeadId { get; set; }
        public int AccountingExpenseHeadId { get; set; }
        public IEnumerable<ProductSubCategoryViewModel> ProductSubCategoriesList { get; set; }
        public List<ProductSubCategoryViewModel> PubCList { get; set; }
    }
}
