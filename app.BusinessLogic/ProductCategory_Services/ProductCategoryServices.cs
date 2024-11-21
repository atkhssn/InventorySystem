using app.EntityModel.CoreModel;
using app.Infrastructure.Repository;
using app.Infrastructure;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Utility;
using app.Infrastructure.Auth;

namespace app.Services.ProductCategory_Services
{
    public class ProductCategoryServices : IProductCategoryServices
    {
        private readonly IEntityRepository<ProductCategory> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public ProductCategoryServices(IEntityRepository<ProductCategory> entityRepository, inventoryDbContext dbContext, IWorkContext workContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<int> AddRecord(ProductCategoryViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name.Trim() == model.Name.Trim()&&f.TrakingId==user.Id);
            if (checkname == null)
            {
                ProductCategory category = new ProductCategory();
                category.Name = model.Name;
                category.ProductType = model.ProductType;
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

        public async Task<ProductCategoryViewModel> GetAllRecord()
        {
            ProductCategoryViewModel model = new ProductCategoryViewModel();
            model.ProductCategoriesList = await Task.Run(() => (from t1 in dbContext.ProductCategory
                                                                where t1.IsActive == true
                                                                select new ProductCategoryViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    Name = t1.Name,
                                                                }).AsQueryable());
            return model;
        }

        public async Task<PagedModel<ProductCategoryViewModel>> GetPagedListAsync(int page, int pageSize, string stringsearch = null)
        {
            ProductCategoryViewModel model = new ProductCategoryViewModel();
            var user= await workContext.GetCurrentUserAsync();
            model.ProductCategoriesList = await Task.Run(() => (from t1 in dbContext.ProductCategory
                                                                //join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                                where t1.IsActive == true
                                                                select new ProductCategoryViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    ProductType = t1.ProductType,
                                                                    Name = t1.Name,
                                                                    UserName= user.FullName,
                                                                    TrakingId=t1.TrakingId,
                                                                }).AsQueryable());
            if (user.UserType == 2)
            {
                model.ProductCategoriesList = model.ProductCategoriesList.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(stringsearch))
            {
                stringsearch = stringsearch.Trim().ToLower();
                model.ProductCategoriesList = model.ProductCategoriesList.Where(t =>
                    t.Name.ToLower().Contains(stringsearch)           
                );
            }
            int resCount = model.ProductCategoriesList.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.ProductCategoriesList.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<ProductCategoryViewModel> pagedModel = new PagedModel<ProductCategoryViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,   
            };
            return pagedModel;
        }

        public async Task<ProductCategoryViewModel> GetProductTypeWiseList(string id)
        {
            ProductCategoryViewModel model = new ProductCategoryViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.ProductCategoriesList = await Task.Run(() => (from t1 in dbContext.ProductCategory
                                                                where t1.IsActive == true && t1.ProductType==id 
                                                                select new ProductCategoryViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    Name = t1.Name,
                                                                }).AsQueryable());
            return model;
        }

        public async Task<ProductCategoryViewModel> GetRecord(long id)
        {
            var result = await _entityRepository.GetByIdAsync(id);
            ProductCategoryViewModel model = new ProductCategoryViewModel();
            model.Id = result.Id;
            model.Name = result.Name;
            model.ProductType = result.ProductType;
            return model;
        }

        public async Task<int> UpdateRecord(ProductCategoryViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name.Trim() == model.Name.Trim()&&f.TrakingId== user.Id && f.Id != model.Id);
            if (checkname == null)
            {
                var result = await _entityRepository.GetByIdAsync(model.Id);
                result.Name = model.Name;
                result.ProductType = model.ProductType;
                await _entityRepository.UpdateAsync(result);
                return 2;
            }
            return 1;
        }
    }
}
