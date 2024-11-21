using app.Services.ProductCategory_Services;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Vendor_Service
{
    public interface IVendorService
    {
        Task<VendorViewModel> GetAllCustomerRecord(int id);
        Task<VendorViewModel> GetAllSupplierRecord(int id);
        Task<int> AddRecord(VendorViewModel model);
        Task<int> UpdateRecord(VendorViewModel model);
        Task<bool> DeleteRecord(long id);
        Task<VendorViewModel> GetRecord(long id);
        Task<PagedModel<VendorViewModel>> GetPagedListAsync(int page, int pageSize, int vendorType, string sarchString);
    }
}
