using app.EntityModel.CoreModel;
using app.Infrastructure.Repository;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Utility.Miscellaneous;
using app.Infrastructure.Auth;
using app.Services.Product_Services;
using Microsoft.EntityFrameworkCore;

namespace app.Services.UserProduct_Service
{
    public class UserProductService : IUserProductService
    {
        private readonly IEntityRepository<UserProduct> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public UserProductService(IEntityRepository<UserProduct> _entityRepository, inventoryDbContext dbContext, IWorkContext workContext)
        {
            this._entityRepository = _entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<bool> AddRecort(long companyid, string userid, long productid)
        {
            var result = dbContext.UserProduct.FirstOrDefault(d => d.ProductId == productid && d.TrakingId == userid);
            if (result!=null)
            {
                bool success = false;   
                if (result.IsActive == true) { success = false; }
                if (result.IsActive == false) { success = true; }
                result.IsActive = success;  
                var res= await _entityRepository.UpdateAsync(result);
                return res;
            }
            UserProduct userProduct=new UserProduct();
            var product= dbContext.Product.FirstOrDefault(d => d.Id == productid);
            userProduct.ProductId = productid;
            userProduct.TrakingId = userid; 
            userProduct.CompanyId=product.CompanyId;
            var getres = await _entityRepository.AddAsync(userProduct);
            if (getres.Id > 0) { return true; }
            else{ return false; } ;

        }

        public async Task<PagedModel<UserProductServiceViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString)
        {

            UserProductServiceViewModel model = new UserProductServiceViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.ProductList = await Task.Run(() => (from t11 in dbContext.UserProduct
                                                      join t1 in dbContext.Product on t11.ProductId equals t1.Id    
                                                      join t2 in dbContext.ProductCategory on t1.ProductCategoryId equals t2.Id
                                                      join t3 in dbContext.ProductSubCategory on t1.ProductSubCategoryId equals t3.Id
                                                      join t4 in dbContext.Users on t1.TrakingId equals t4.Id
                                                      join t5 in dbContext.Company on t1.CompanyId equals t5.Id
                                                      where t11.IsActive == true&&t11.TrakingId==user.Id
                                                      select new UserProductServiceViewModel
                                                      {
                                                          Id = t11.Id,
                                                          ProductCategoryName = t2.Name,
                                                          ProductSubCategoryName = t3.Name,                                                         
                                                          ProductName = t1.ProductName,
                                                          MRP=t11.MRP,
                                                          AVGPrice = t11.AVGPrice,  
                                                          UnitPrice = t11.UnitPrice,    
                                                          CompanyId = t1.CompanyId,
                                                          CompanyName = t5.Name

                                                      }).AsQueryable());
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.ProductList = model.ProductList.Where(t =>
                    t.ProductName.ToLower().Contains(sarchString) ||
                    t.UserName.ToLower().Contains(sarchString) ||
                    t.ProductCategoryName.ToLower().Contains(sarchString) ||
                    t.ProductSubCategoryName.ToLower().Contains(sarchString) ||
                    t.CompanyName.ToLower().Contains(sarchString) 

                );
            }
            int resCount = model.ProductList.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.ProductList.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<UserProductServiceViewModel> pagedModel = new PagedModel<UserProductServiceViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;

        }

        public async Task<bool> updatet(long id, decimal mrp)
        {
            var userproduct= await dbContext.UserProduct.FirstOrDefaultAsync(f=>f.Id==id);
            userproduct.MRP = mrp;
            var res = await _entityRepository.UpdateAsync(userproduct);
           return res;  
        }
    }
}
