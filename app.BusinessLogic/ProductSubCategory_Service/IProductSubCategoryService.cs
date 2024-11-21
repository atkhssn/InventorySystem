using app.Services.ProductCategory_Services;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.ProductSubCategory_Service
{
    public interface IProductSubCategoryService
    {
        Task<int> AddRecord(ProductSubCategoryViewModel model);
        Task<int> UpdateRecord(ProductSubCategoryViewModel model);
        Task<bool> DeleteRecord(long id);
        Task<ProductSubCategoryViewModel> GetRecord(long id);
        Task<ProductSubCategoryViewModel> GetProductTypeWiseList(long id);
        Task<PagedModel<ProductSubCategoryViewModel>> GetPagedListAsync(int page, int pageSize,int ProductCategoryId,string ProductType, string sarchString);
    }
}
