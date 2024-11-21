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
using Microsoft.EntityFrameworkCore;
using app.Services.ProductCategory_Services;

namespace app.Services.ProductSubCategory_Service
{
    public class ProductSubCategoryService : IProductSubCategoryService
    {
        private readonly IEntityRepository<ProductSubCategory> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public ProductSubCategoryService(IEntityRepository<ProductSubCategory> entityRepository,
            inventoryDbContext dbContext,
            IWorkContext workContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<int> AddRecord(ProductSubCategoryViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name.Trim() == model.Name.Trim()&&f.TrakingId==user.Id&&f.ProductCategoryId==model.ProductCategoryId);
            if (checkname == null)
            {
                ProductSubCategory category = new ProductSubCategory();
                var productCategory = await dbContext.ProductCategory.FindAsync(model.ProductCategoryId);
                category.Name = model.Name;
                category.ProductCategoryId = model.ProductCategoryId;
                category.ProductType = productCategory.ProductType;
                var res = await _entityRepository.AddAsync(category);
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

        public async Task<PagedModel<ProductSubCategoryViewModel>> GetPagedListAsync(int page, int pageSize, int ProductCategoryId, string ProductType, string sarchString)
        {
            ProductSubCategoryViewModel model = new ProductSubCategoryViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.ProductSubCategoriesList = await Task.Run(() => (from t1 in dbContext.ProductSubCategory
                                                                  join t3 in dbContext.ProductCategory on t1.ProductCategoryId equals t3.Id
                                                                  join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                                  where t1.IsActive == true
                                                                select new ProductSubCategoryViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    ProductType = t1.ProductType,
                                                                    Name = t1.Name,
                                                                    UserName = t2.FullName,
                                                                    TrakingId = t1.TrakingId,
                                                                    ProductCategoryId = t1.ProductCategoryId,
                                                                    ProductCategoryName=t3.Name
                                                                }).AsQueryable());
            if (user.UserType == 2)
            {
                model.ProductSubCategoriesList = model.ProductSubCategoriesList.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (ProductCategoryId>0)
            {
                model.ProductSubCategoriesList = model.ProductSubCategoriesList.Where(f => f.ProductCategoryId == ProductCategoryId).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(ProductType))
            {
                model.ProductSubCategoriesList = model.ProductSubCategoriesList.Where(f => f.ProductType.Trim() == ProductType.Trim()).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.ProductSubCategoriesList = model.ProductSubCategoriesList.Where(t =>
                    t.Name.ToLower().Contains(sarchString) ||
                    t.UserName.ToLower().Contains(sarchString) ||
                    t.ProductCategoryName.ToLower().Contains(sarchString)                  
                );
            }
            int resCount = model.ProductSubCategoriesList.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.ProductSubCategoriesList.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<ProductSubCategoryViewModel> pagedModel = new PagedModel<ProductSubCategoryViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<ProductSubCategoryViewModel> GetProductTypeWiseList(long id)
        {
            var user = await workContext.GetCurrentUserAsync();
            ProductSubCategoryViewModel model = new ProductSubCategoryViewModel();
            model.ProductSubCategoriesList = await Task.Run(() => (from t1 in dbContext.ProductSubCategory
                                                                where t1.IsActive == true && t1.ProductCategoryId == id
                                                                   select new ProductSubCategoryViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    Name = t1.Name,
                                                                }).AsQueryable());
            return model;

        }

        public async Task<ProductSubCategoryViewModel> GetRecord(long id)
        {
            var result = await _entityRepository.GetByIdAsync(id);
            ProductSubCategoryViewModel model = new ProductSubCategoryViewModel();
            model.Id = result.Id;
            model.Name = result.Name;
            model.ProductType = result.ProductType;
            model.ProductCategoryId = result.ProductCategoryId;
            return model;
        }

        public async Task<int> UpdateRecord(ProductSubCategoryViewModel model)
        {

            var user = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name.Trim() == model.Name.Trim()&&f.TrakingId==user.Id && f.ProductCategoryId == model.ProductCategoryId&&f.Id!=model.Id);
            if (checkname == null)
            {
                 
                ProductSubCategory category = await dbContext.ProductSubCategory.FirstOrDefaultAsync(d => d.Id == model.Id);
                var productCategory = await dbContext.ProductCategory.FindAsync(model.ProductCategoryId);
                category.Name = model.Name;
                category.ProductCategoryId = model.ProductCategoryId;
                category.ProductType = productCategory.ProductType;
                var res = await _entityRepository.UpdateAsync(category);
                return 2;
            }
            return 1;
        }
    }
}
