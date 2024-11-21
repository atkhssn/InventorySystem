using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.ProductCategory_Services
{
    public interface IProductCategoryServices
    {
        Task<ProductCategoryViewModel> GetAllRecord();
        Task<int> AddRecord( ProductCategoryViewModel model);
        Task<int> UpdateRecord( ProductCategoryViewModel model);
        Task<bool> DeleteRecord( long id );
        Task<ProductCategoryViewModel> GetRecord(long id);
        Task<ProductCategoryViewModel> GetProductTypeWiseList(string id);
        Task<PagedModel<ProductCategoryViewModel>> GetPagedListAsync(int page, int pageSize,string stringsearch);
    }
}
