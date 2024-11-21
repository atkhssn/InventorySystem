using app.Services.MainMenuService;
using app.Services.Product_Services;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.UserProduct_Service
{
    public interface IUserProductService
    {
        Task<bool> AddRecort(long companyid,string userid,long productid);
        Task<bool> updatet(long id,decimal mrp);
        Task<PagedModel<UserProductServiceViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString);
    }
}
