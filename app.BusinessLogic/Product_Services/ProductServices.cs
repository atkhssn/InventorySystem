using app.EntityModel.CoreModel;
using app.Infrastructure.Auth;
using app.Infrastructure.Repository;
using app.Infrastructure;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Services.ProductSubCategory_Service;
using Microsoft.EntityFrameworkCore;
using app.Utility;

namespace app.Services.Product_Services
{
    public class ProductServices : IProductServices
    {
        private readonly IEntityRepository<Product> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public ProductServices(IEntityRepository<Product> entityRepository, inventoryDbContext dbContext, IWorkContext workContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<int> AddRecord(ProductViewModel model)
        {
            var users = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.ProductName.Trim() == model.ProductName.Trim()&&f.TrakingId==users.Id&&f.ProductCategoryId==model.ProductCategoryId);
            if (checkname == null)
            {
                var user = await workContext.GetCurrentUserAsync();
                var count = await dbContext.Product.Where(d => d.TrakingId == user.Id).CountAsync();               
                Product product = new Product();
                product.ProductName = model.ProductName;    
                product.ShortName=model.ShortName;
                product.ProductCategoryId = model.ProductCategoryId;
                product.ProductSubCategoryId=model.ProductSubCategoryId;
                product.UnitName=model.UnitName;
                product.ProductType=model.ProductType;
                product.CompanyId=model.CompanyId;
                product.ProductCode = GenerateProductCode(model.ProductType,count.ToString());
               await _entityRepository.AddAsync(product);
                return 2;
            }
            return 1;   
        }

        public async Task<bool> DeleteRecord(long id)
        {
            var result = await _entityRepository.GetByIdAsync(id);
            result.IsActive = false;
            await _entityRepository.UpdateAsync(result);
            return true;
        }

        public async Task<PagedModel<ProductViewModel>> GetPagedListAsync(int page, int pageSize, int ProductCategoryId, int ProductSubCategoryId, string ProductType, string sarchString)
        {

            ProductViewModel model = new ProductViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.ProductList = await Task.Run(() => (from t1 in dbContext.Product
                                                                   join t2 in dbContext.ProductCategory on t1.ProductCategoryId equals t2.Id
                                                                   join t3 in dbContext.ProductSubCategory on t1.ProductSubCategoryId equals t3.Id
                                                                   join t4 in dbContext.Users on t1.TrakingId equals t4.Id
                                                                   join t5 in dbContext.Company on t1.CompanyId equals t5.Id
                                                                   where t1.IsActive == true
                                                                   select new ProductViewModel
                                                                   {
                                                                       Id= t1.Id,
                                                                       ProductCategoryId= t1.ProductCategoryId,
                                                                       ProductSubCategoryId= t1.ProductSubCategoryId,
                                                                       ProductCategoryName=t2.Name,
                                                                       ProductType=t1.ProductType,
                                                                       ProductSubCategoryName=t3.Name,
                                                                       UserName=t4.FullName,
                                                                       UserEmail=t4.Email,
                                                                       UnitName=t1.UnitName,
                                                                       ProductName=t1.ProductName,
                                                                       ProductCode=t1.ProductCode,
                                                                       CompanyId=t1.CompanyId,
                                                                       CompanyName=t5.Name
                                                                       
                                                                   }).AsQueryable());
            if (user.UserType == 2)
            {
                model.ProductList = model.ProductList.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (ProductCategoryId > 0)
            {
                model.ProductList = model.ProductList.Where(f => f.ProductCategoryId == ProductCategoryId).AsQueryable();
            }
            if (ProductSubCategoryId > 0)
            {
                model.ProductList = model.ProductList.Where(f => f.ProductSubCategoryId == ProductSubCategoryId).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(ProductType))
            {
                model.ProductList = model.ProductList.Where(f => f.ProductType.Trim() == ProductType.Trim()).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.ProductList = model.ProductList.Where(t =>
                    t.ProductName.ToLower().Contains(sarchString) ||
                    t.UserName.ToLower().Contains(sarchString) ||
                    t.ProductCategoryName.ToLower().Contains(sarchString)||
                    t.ProductSubCategoryName.ToLower().Contains(sarchString)||
                    t.UnitName.ToLower().Contains(sarchString)||
                    t.ProductCode.ToLower().Contains(sarchString)

                );
            }
            int resCount = model.ProductList.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.ProductList.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<ProductViewModel> pagedModel = new PagedModel<ProductViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<ProductViewModel> GetRecord(long id)
        {
            var result = await _entityRepository.GetByIdAsync(id);
            ProductViewModel model = new ProductViewModel();    
            model.ProductName = result.ProductName;
            model.ShortName = result.ShortName;
            model.Id=result.Id;
            model.UnitName = result.UnitName;   
            model.ProductType = result.ProductType; 
            model.ProductCategoryId = result.ProductCategoryId; 
            model.ProductSubCategoryId = result.ProductSubCategoryId;   
            model.CompanyId = result.CompanyId;   
            return model;
        }

        public async Task<int> UpdateRecord(ProductViewModel model)
        {
            var users = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.ProductName.Trim() == model.ProductName.Trim()&& 
            f.TrakingId == users.Id &&
            f.ProductCategoryId == model.ProductCategoryId&&
            f.ProductSubCategoryId==model.ProductSubCategoryId&&
            f.Id!=model.Id);
            if (checkname == null)
            {
                Product product = await _entityRepository.GetByIdAsync(model.Id);
                product.ProductName = model.ProductName;
                product.ShortName = model.ShortName;
                product.CompanyId = model.CompanyId;
                product.ProductCategoryId = model.ProductCategoryId;
                product.ProductSubCategoryId = model.ProductSubCategoryId;
                product.UnitName = model.UnitName;
                product.ProductType = model.ProductType;
                await _entityRepository.UpdateAsync(product);
                return 2;
            }
            return 1;
        }

        private string GenerateProductCode(string type, string lastProductCode)
        {
           // string productCodeNumber = lastProductCode.Length < 7 ? "0" : lastProductCode.Substring(1, 7);
            int productNumber = Convert.ToInt32(lastProductCode);
            productNumber= productNumber+1;
            return type + productNumber.ToString().PadLeft(7, '0');
        }


    }
}
