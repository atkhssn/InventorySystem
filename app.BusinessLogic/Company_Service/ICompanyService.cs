using app.Services.ProductCategory_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.Company_Service
{
    public interface ICompanyService
    {
        Task<CompanyViewModel> GetAllRecord();
        Task<int> AddRecord(CompanyViewModel model);
        Task<int> UpdateRecord(CompanyViewModel model);
        Task<bool> DeleteRecord(long id);
        Task<CompanyViewModel> GetRecord(long id);
    }
}
