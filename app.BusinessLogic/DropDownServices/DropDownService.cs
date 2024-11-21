using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using app.Services.Product_Services;
using app.Services.UserProduct_Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.DropDownServices
{
    public class DropDownService : IDropDownService
    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public DropDownService(inventoryDbContext dbContext, IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<IEnumerable<DropDownViewmodel>> Companlist()
        {
            var user = await workContext.GetCurrentUserAsync();
            IEnumerable<DropDownViewmodel> dropDownViewmodels = await Task.Run(() => (from t1 in dbContext.Company
                                                                                      select new DropDownViewmodel
                                                                                      {
                                                                                          Id = t1.Id,
                                                                                          Name = t1.Name
                                                                                      }).AsQueryable());
            return dropDownViewmodels;
        }
    

        public async Task<IEnumerable<DropDownViewmodel>> productlist()
        {
            var user = await workContext.GetCurrentUserAsync();
            IEnumerable<DropDownViewmodel> dropDownViewmodels = await Task.Run(() => 
                                                      (from t11 in dbContext.UserProduct
                                                      join t1 in dbContext.Product on t11.ProductId equals t1.Id
                                                      join t2 in dbContext.ProductCategory on t1.ProductCategoryId equals t2.Id
                                                      join t3 in dbContext.ProductSubCategory on t1.ProductSubCategoryId equals t3.Id
                                                      join t4 in dbContext.Company on t11.CompanyId equals t4.Id
                                                      where t11.IsActive == true&&t11.TrakingId==user.Id
                                                      select new DropDownViewmodel
                                                      {
                                                          Id = t1.Id,
                                                          Name=t4.Name+" -- "+ t2.Name+" - "+t3.Name+" - "+t1.ProductName+" ("+t1.UnitName+")",
                                                      }).AsQueryable());
            return dropDownViewmodels;
        }

        public async Task<Product> sigleproduct(long id)
        {
            Product product= await dbContext.Product.FindAsync(id);
            return product;   
        }

        public async Task<List<DropDownCustomViewModel>> companyproductlist(long id)
        {
            var user = await workContext.GetCurrentUserAsync();
            List<DropDownCustomViewModel> dropDownViewmodels = await Task.Run(() => (from t1 in dbContext.Product
                                                                                      join t2 in dbContext.ProductCategory on t1.ProductCategoryId equals t2.Id
                                                                                      join t3 in dbContext.ProductSubCategory on t1.ProductSubCategoryId equals t3.Id
                                                                                      where t1.IsActive == true && t1.CompanyId == id
                                                                                      select new DropDownCustomViewModel
                                                                                      {
                                                                                          Id = t1.Id,
                                                                                          Name = t2.Name + " - " + t3.Name + " - " + t1.ProductName + " (" + t1.UnitName + ")",
                                                                                      }).ToListAsync());
            foreach (var item in dropDownViewmodels)
            {
                var userproduct=  dbContext.UserProduct.FirstOrDefault(f=>f.ProductId==item.Id&&f.IsActive==true&&f.TrakingId==user.Id);
                if (userproduct != null)
                {
                    item.IsAssigen= true;
                }
                else
                {
                    item.IsAssigen= false;  
                }
            }
            return dropDownViewmodels;
        }

        public async Task<IEnumerable<DropDownViewmodel>> vendorlist(int vendortype)
        {
            var users = await workContext.GetCurrentUserAsync();
            IEnumerable<DropDownViewmodel> dropDownViewmodels  = (from t1 in dbContext.Vendor.Where(x => x.VendorType == vendortype&&x.TrakingId==users.Id)
                     where t1.IsActive 
                     select new DropDownViewmodel
                     {
                         Name = "[" + t1.Name + "] " + t1.Mobile,
                         Id = t1.Id
                     }).OrderBy(x => x.Name).AsQueryable();

            return dropDownViewmodels;
        }

        public async Task<Vendor> sigleCustomer(string mobile)
        {
            Vendor vendor = new Vendor();
            var users = await workContext.GetCurrentUserAsync();
            Vendor res = await dbContext.Vendor.FirstOrDefaultAsync(x => x.Mobile.Trim() == mobile.Trim()&&x.TrakingId== users.Id);   
            if (res != null)
            {
                return res;
            }
            vendor.Id = 0;
            return vendor;
            
        }

        public async Task<UserProductServiceViewModel> sigleproductWithstock(long id)
        {
            UserProductServiceViewModel model = new UserProductServiceViewModel();
            var users = await workContext.GetCurrentUserAsync();
            var userproduct = await dbContext.UserProduct.FirstOrDefaultAsync(k=>k.ProductId==id&&k.TrakingId== users.Id);
            var product = await dbContext.Product.FindAsync(id);
            var stock = dbContext.StockInfo.Where(f=>f.ProductId==id&& f.TrakingId == users.Id).Sum(f=>f.InQty-f.OutQty);
            //var stock = dbContext.StockView.FromSqlRaw("EXEC dbo.SP_Stock @p0", users.Id).Where(f => f.ProductId == id).Sum(f => f.InQty - f.OutQty);
            model.ProductId=userproduct.ProductId;
            model.MRP=userproduct.MRP;
            model.AVGPrice=userproduct.AVGPrice;
            model.UnitNane=product.UnitName;
            model.StockQuntity= stock;
            return model;   
        }

        public async Task<IEnumerable<DropDownCustomViewModel>> companyUserproductlist(long id)
        {
            var user = await workContext.GetCurrentUserAsync();
            IEnumerable<DropDownCustomViewModel> dropDownViewmodels = await Task.Run(() =>
                                                      (from t11 in dbContext.UserProduct
                                                       join t1 in dbContext.Product on t11.ProductId equals t1.Id
                                                       join t2 in dbContext.ProductCategory on t1.ProductCategoryId equals t2.Id
                                                       join t3 in dbContext.ProductSubCategory on t1.ProductSubCategoryId equals t3.Id
                                                       join t4 in dbContext.Company on t11.CompanyId equals t4.Id
                                                       where t11.IsActive == true && t11.TrakingId == user.Id&&t11.CompanyId==id
                                                       select new DropDownCustomViewModel
                                                       {
                                                           Id = t1.Id,
                                                           Name = t4.Name + " -- " + t2.Name + " - " + t3.Name + " - " + t1.ProductName + " (" + t1.UnitName + ")",
                                                       }).AsQueryable());
            return dropDownViewmodels;
        }

        public async Task<Vendor> sigleCustomerinfo(long id)
        {
            Vendor vendor = new Vendor();
            var users = await workContext.GetCurrentUserAsync();
            Vendor res = await dbContext.Vendor.FirstOrDefaultAsync(x => x.Id==id && x.TrakingId == users.Id);
            if (res != null)
            {
                return res;
            }
            vendor.Id = 0;
            return vendor;
        }

        public async Task<List<DropDownCustomViewModel>> AdvanceSurachproductlist(long catagoryId, long subcatagoryid, long companyid)
        {
            var user = await workContext.GetCurrentUserAsync();
            List<DropDownCustomViewModel> dropDownViewmodels = await Task.Run(() => (from t1 in dbContext.Product
                                                                                     join t2 in dbContext.ProductCategory on t1.ProductCategoryId equals t2.Id
                                                                                     join t3 in dbContext.ProductSubCategory on t1.ProductSubCategoryId equals t3.Id
                                                                                     join t4 in dbContext.Company on t1.CompanyId equals t4.Id
                                                                                     where t1.IsActive == true
                                                                                     && (catagoryId == 0|| t1.ProductCategoryId == catagoryId)
                                                                                     && (subcatagoryid == 0 || t1.ProductSubCategoryId == subcatagoryid)
                                                                                     && (companyid == 0 || t1.CompanyId == companyid)
                                                                                     select new DropDownCustomViewModel
                                                                                     {
                                                                                         Id = t1.Id,
                                                                                         Name = t1.ProductName + " (" + t1.UnitName + ")",
                                                                                         PCName=t2.Name,
                                                                                         PSCName=t3.Name,
                                                                                         CName=t4.Name,

                                                                                     }).ToListAsync());
            foreach (var item in dropDownViewmodels)
            {
                var userproduct = dbContext.UserProduct.FirstOrDefault(f => f.ProductId == item.Id && f.IsActive == true && f.TrakingId == user.Id);
                if (userproduct != null)
                {
                    item.IsAssigen = true;
                }
                else
                {
                    item.IsAssigen = false;
                }
            }
            return dropDownViewmodels;
        }

    }
}
