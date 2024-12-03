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
            CostCentersViewModel request = new CostCentersViewModel();
            request.CostCenterViewModels = await _dbContext.CostCenters.Where(x => x.IsActive).Select(x => new CostCentersViewModel
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
            return request;
        }

        public async Task<CostCentersViewModel> CostCenterAync(long id)
        {
            CostCentersViewModel request = new CostCentersViewModel();
            request = await _dbContext.CostCenters.Where(x => x.Id.Equals(id) && x.IsActive).Select(x => new CostCentersViewModel
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
            return request;
        }

        public async Task<ResponseViewModel> AddCostCenterAsync(CostCentersViewModel model)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Name.ToUpper().Equals(model.Name.ToUpper())).FirstOrDefault();
            var findCostCenterShort = _dbContext.CostCenters.Where(x => x.ShortName.ToUpper().Equals(model.ShortName.ToUpper())).FirstOrDefault();

            if (findCostCenter is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => request);
            }

            if (findCostCenterShort is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.ShortName}] has already been added.";
                return await Task.Run(() => request);
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
                request.ResponseCode = 200;
                request.ResponseMessage = $"New cost center has been added.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> UpdateCostCenterAync(CostCentersViewModel model)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var getCostCenter = _dbContext.CostCenters.Where(x => x.Id.Equals(model.Id) && x.ShortName.ToUpper().Equals(model.ShortName.ToUpper())).FirstOrDefault();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Name.ToUpper().Equals(model.Name.ToUpper())).FirstOrDefault();

            if (getCostCenter is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => request);
            }

            if (findCostCenter is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => request);
            }

            getCostCenter.Name = model.Name;
            getCostCenter.UpdatedBy = user.FullName;
            getCostCenter.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"Cost center has been modified.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> DeleteCostCenterAync(long id)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (findCostCenter is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => request);
            }

            findCostCenter.IsActive = false;
            findCostCenter.UpdatedBy = user.FullName;
            findCostCenter.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"Cost center has been removed.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        #endregion

    }
}
