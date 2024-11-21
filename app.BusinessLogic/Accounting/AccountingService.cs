using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using app.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace app.Services.Accounting
{
    public class AccountingService : IAccountingService
    {
        private readonly IEntityRepository<CostCenter> _entityRepository;
        private readonly inventoryDbContext _dbContext;
        private readonly IWorkContext _workContext;
        public AccountingService(IWorkContext workContext, IEntityRepository<CostCenter> entityRepository, inventoryDbContext dbContext)
        {
            _entityRepository = entityRepository;
            _dbContext = dbContext;
            _workContext = workContext;
        }

        #region Cost Center

        public async Task<CostCenterViewModel> GetAllRecordAsync()
        {
            CostCenterViewModel model = new CostCenterViewModel();

            model.CostCenterViewModels = await _dbContext.CostCenter.Where(x => x.IsActive).Select(x => new CostCenterViewModel
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Name = x.Name,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = DateTime.Now,
                IsActive = true
            }).ToListAsync();

            return model;
        }

        public async Task<CostCenterViewModel> GetRecordDetailAync(long Id)
        {
            CostCenterViewModel model = new CostCenterViewModel();
            model = await _dbContext.CostCenter.Where(x => x.Id == Id && x.IsActive).Select(x => new CostCenterViewModel
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Name = x.Name,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = DateTime.Now,
                IsActive = true
            }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> AddRecordAsync(CostCenterViewModel model)
        {
            var getCostCenter = _dbContext.CostCenter.Where(x => x.Name == model.Name).FirstOrDefault();
            var user = await _workContext.GetCurrentUserAsync();

            if (getCostCenter is not null)
            {
                return await Task.Run(() => false);
            }

            CostCenter costCenter = new CostCenter
            {
                Id = model.Id,
                ShortName = model.ShortName.ToUpper(),
                Name = model.Name,
                TrakingId = user.UserName,
                CreatedBy = user.FullName,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _dbContext.CostCenter.Add(costCenter);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRecordAync(CostCenterViewModel model)
        {
            var getCostCenter = _dbContext.CostCenter.Where(x => x.Id == model.Id).FirstOrDefault();
            var user = await _workContext.GetCurrentUserAsync();

            if (getCostCenter is null)
            {
                return await Task.Run(() => false);
            }

            if (getCostCenter.Name.ToUpper() == model.Name.ToUpper())
            {
                return await Task.Run(() => false);
            }

            getCostCenter.ShortName = model.ShortName.ToUpper();
            getCostCenter.Name = model.Name;
            getCostCenter.UpdatedBy = user.FullName;
            getCostCenter.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRecordAync(long Id)
        {
            var getCostCenter = _dbContext.CostCenter.Where(x => x.Id == Id).FirstOrDefault();
            var user = await _workContext.GetCurrentUserAsync();

            if (getCostCenter is null)
            {
                return await Task.Run(() => false);
            }

            getCostCenter.IsActive = false;
            getCostCenter.UpdatedBy = user.FullName;
            getCostCenter.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        #endregion

    }
}
