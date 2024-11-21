using app.EntityModel.CoreModel;
using app.Infrastructure.Auth;
using app.Infrastructure.Repository;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Services.ProductCategory_Services;

namespace app.Services.Company_Service
{
    public class CompanyServices : ICompanyService
    {
        private readonly IEntityRepository<Company> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public CompanyServices(IEntityRepository<Company> entityRepository, inventoryDbContext dbContext, IWorkContext workContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<int> AddRecord(CompanyViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name.Trim() == model.Name.Trim());
            if (checkname == null)
            {
                Company com = new Company();
                com.Name = model.Name;
                var res = await _entityRepository.AddAsync(com);
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

        public async Task<CompanyViewModel> GetAllRecord()
        {
            CompanyViewModel model = new CompanyViewModel();
            model.CompanyList = await Task.Run(() => (from t1 in dbContext.Company
                                                                where t1.IsActive == true
                                                                select new CompanyViewModel
                                                                {
                                                                    Id = t1.Id,
                                                                    Name = t1.Name,
                                                                }).AsQueryable());
            return model;
        }

        public async Task<CompanyViewModel> GetRecord(long id)
        {
            var result = await _entityRepository.GetByIdAsync(id);
            CompanyViewModel model = new CompanyViewModel();
            model.Id = result.Id;
            model.Name = result.Name;
            return model;
        }

        public async Task<int> UpdateRecord(CompanyViewModel model)
        {

            var checkname = _entityRepository.AllIQueryableAsync().FirstOrDefault(f => f.Name.Trim() == model.Name.Trim());
            if (checkname == null)
            {
                var result = await _entityRepository.GetByIdAsync(model.Id);
                result.Name = model.Name;
                await _entityRepository.UpdateAsync(result);
                return 2;
            }
            return 1;
        }
    }
}
