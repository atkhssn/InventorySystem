using app.Services.ProductSubCategory_Service;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Product_Services
{
    public interface IProductServices
    {
        Task<int> AddRecord(ProductViewModel model);
        Task<int> UpdateRecord(ProductViewModel model);
        Task<bool> DeleteRecord(long id);
        Task<ProductViewModel> GetRecord(long id);
        Task<PagedModel<ProductViewModel>> GetPagedListAsync(int page, int pageSize, int ProductCategoryId, int ProductSubCategoryId, string ProductType, string sarchString);
    }
}
