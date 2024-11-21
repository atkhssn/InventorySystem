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

        public async Task<ResponseViewModel> AddRecordAsync(CostCenterViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenter.Where(x => x.Name.ToUpper() == model.Name.ToUpper()).FirstOrDefault();
            var findCostCenterShort = _dbContext.CostCenter.Where(x => x.ShortName.ToUpper() == model.ShortName.ToUpper()).FirstOrDefault();

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

        public async Task<ResponseViewModel> UpdateRecordAync(CostCenterViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var getCostCenter = _dbContext.CostCenter.Where(x => x.Id == model.Id && x.ShortName.ToUpper() == model.ShortName.ToUpper()).FirstOrDefault();
            var findCostCenter = _dbContext.CostCenter.Where(x => x.Name.ToUpper() == model.Name.ToUpper()).FirstOrDefault();

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

        public async Task<ResponseViewModel> DeleteRecordAync(long Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenter.Where(x => x.Id == Id).FirstOrDefault();

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
