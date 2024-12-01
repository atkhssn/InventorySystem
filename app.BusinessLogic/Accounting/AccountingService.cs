using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace app.Services.Accounting
{
    public class AccountingService : IAccountingService
    {
        private readonly inventoryDbContext _dbContext;
        private readonly IWorkContext _workContext;
        public AccountingService(IWorkContext workContext, inventoryDbContext dbContext)
        {
            _dbContext = dbContext;
            _workContext = workContext;
        }

        #region Cost Center

        public async Task<CostCentersViewModel> CostCentersAsync()
        {
            CostCentersViewModel model = new CostCentersViewModel();
            model.CostCenterViewModels = await _dbContext.CostCenters.Where(x => x.IsActive).Select(x => new CostCentersViewModel
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

        public async Task<CostCentersViewModel> CostCenterAync(long Id)
        {
            CostCentersViewModel model = new CostCentersViewModel();
            model = await _dbContext.CostCenters.Where(x => x.Id == Id && x.IsActive).Select(x => new CostCentersViewModel
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

        public async Task<ResponseViewModel> AddCostCenterAsync(CostCentersViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Name.ToUpper() == model.Name.ToUpper()).FirstOrDefault();
            var findCostCenterShort = _dbContext.CostCenters.Where(x => x.ShortName.ToUpper() == model.ShortName.ToUpper()).FirstOrDefault();

            if (findCostCenter is not null)
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => response);
            }

            if (findCostCenterShort is not null)
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"[{model.ShortName}] has already been added.";
                return await Task.Run(() => response);
            }

            CostCenters costCenter = new CostCenters
            {
                Id = model.Id,
                ShortName = model.ShortName.ToUpper(),
                Name = model.Name,
                TrakingId = user.UserName,
                CreatedBy = user.FullName,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _dbContext.CostCenters.Add(costCenter);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"New cost center has been added.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        public async Task<ResponseViewModel> UpdateCostCenterAync(CostCentersViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var getCostCenter = _dbContext.CostCenters.Where(x => x.Id == model.Id && x.ShortName.ToUpper() == model.ShortName.ToUpper()).FirstOrDefault();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Name.ToUpper() == model.Name.ToUpper()).FirstOrDefault();

            if (getCostCenter is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => response);
            }

            if (findCostCenter is not null)
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => response);
            }

            getCostCenter.Name = model.Name;
            getCostCenter.UpdatedBy = user.FullName;
            getCostCenter.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"Cost center has been modified.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        public async Task<ResponseViewModel> DeleteCostCenterAync(long Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Id == Id).FirstOrDefault();

            if (findCostCenter is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => response);
            }

            findCostCenter.IsActive = false;
            findCostCenter.UpdatedBy = user.FullName;
            findCostCenter.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"Cost center has been removed.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        #endregion

    }
}
