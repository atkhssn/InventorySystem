using app.EntityModel.CoreModel;
using app.Services.UserProduct_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.DropDownServices
{
    public interface IDropDownService
    {
        Task<IEnumerable<DropDownViewmodel>> vendorlist(int vendortype);
        Task<IEnumerable<DropDownViewmodel>> productlist();
        Task<List<DropDownCustomViewModel>> companyproductlist(long id);
        Task<List<DropDownCustomViewModel>> AdvanceSurachproductlist(long catagoryId,long subcatagoryid,long companyid);
        Task<IEnumerable<DropDownCustomViewModel>> companyUserproductlist(long id);
        Task<IEnumerable<DropDownViewmodel>> Companlist();
        Task<Product> sigleproduct(long id);
        Task<UserProductServiceViewModel> sigleproductWithstock(long id);
        Task<Vendor> sigleCustomer(string mobile);
        Task<Vendor> sigleCustomerinfo(long id);
    }
}
