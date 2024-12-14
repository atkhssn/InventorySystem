using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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

        #region Chart of Accounting

        public async Task<ChartOfAccountsViewModel> ChartOfAccoutingsAsync()
        {
            ChartOfAccountsViewModel request = new ChartOfAccountsViewModel();
            request.ChartOfAccountsViewModels = await _dbContext.ChartOfAccounts.Where(x => x.IsActive).Select(x => new ChartOfAccountsViewModel
            {
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                ParentAccountCode = x.ParentAccountCode,
                Level = x.Level,
                CoATypeId = x.CoATypeId,
                IsRoot = x.IsRoot,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = x.UpdatedOn,
                IsActive = true
            }).ToListAsync();
            return request;
        }

        public async Task<ResponseViewModel> AddAccountHeadAsync(ChartOfAccountsViewModel model)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var fetchCoAList = await _dbContext.ChartOfAccounts.Where(x => x.IsActive).ToListAsync();
            var findParentHead = fetchCoAList.Where(x => x.AccountCode.Equals(model.ParentAccountCode)).FirstOrDefault();

            if (findParentHead is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"Parent code [{model.ParentAccountCode}] not found.";
                return await Task.Run(() => request);
            }

            if (fetchCoAList.Where(x => x.ParentAccountCode.Equals(findParentHead.AccountCode) && x.AccountCode.Equals(model.AccountCode)).FirstOrDefault() is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.AccountCode}] has already been added.";
                return await Task.Run(() => request);
            }

            if (fetchCoAList.Where(x => x.ParentAccountCode.Equals(findParentHead.AccountCode) && x.AccountName.Equals(model.AccountName)).FirstOrDefault() is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.AccountName}] has already been added.";
                return await Task.Run(() => request);
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    ChartOfAccounts chartOfAccounts = new ChartOfAccounts
                    {
                        AccountCode = model.AccountCode,
                        AccountName = model.AccountName,
                        ParentAccountCode = model.ParentAccountCode,
                        Level = findParentHead.Level + 1,
                        CoATypeId = findParentHead.CoATypeId,
                        CreatedBy = user.FullName,
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    _dbContext.ChartOfAccounts.Add(chartOfAccounts);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        request.ResponseCode = 200;
                        request.ResponseMessage = "New account head has been added.";
                        transaction.Complete();
                    }
                    else
                    {
                        request.ResponseCode = 500;
                        request.ResponseMessage = "An internal server error occurred.";
                    }
                }
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> UpdateAccountHeadAsync(ChartOfAccountsViewModel model)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var fetchCoAList = await _dbContext.ChartOfAccounts.Where(x => x.IsActive).ToListAsync();
            var findHead = fetchCoAList.Where(x => x.AccountCode.Equals(model.AccountCode)).FirstOrDefault();

            if (findHead is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"Account code [{model.AccountCode}] not found.";
                return await Task.Run(() => request);
            }

            if (fetchCoAList.Where(x => x.ParentAccountCode.Equals(findHead.ParentAccountCode) && x.AccountName.Equals(model.AccountName)).FirstOrDefault() is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.AccountName}] has already been added.";
                return await Task.Run(() => request);
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    findHead.AccountName = model.AccountName;
                    findHead.UpdatedBy = user.FullName;
                    findHead.UpdatedOn = DateTime.Now;

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        request.ResponseCode = 200;
                        request.ResponseMessage = "Account head has been modified.";
                        transaction.Complete();
                    }
                    else
                    {
                        request.ResponseCode = 500;
                        request.ResponseMessage = "An internal server error occurred.";
                    }
                }
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> DeleteAccountHeadAsync(string accountCode)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var fetchCoAList = await _dbContext.ChartOfAccounts.Where(x => x.IsActive).ToListAsync();
            var findHead = fetchCoAList.Where(x => x.AccountCode.Equals(accountCode)).FirstOrDefault();

            if (findHead is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"Account code [{accountCode}] not found.";
                return await Task.Run(() => request);
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    findHead.IsActive = false;
                    findHead.UpdatedBy = user.FullName;
                    findHead.UpdatedOn = DateTime.Now;

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        request.ResponseCode = 200;
                        request.ResponseMessage = "Account head has been removed.";
                        transaction.Complete();
                    }
                    else
                    {
                        request.ResponseCode = 500;
                        request.ResponseMessage = "An internal server error occurred.";
                    }
                }
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
            return await Task.Run(() => request);
        }

        #endregion

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

            try
            {
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
                else
                {
                    request.ResponseCode = 500;
                    request.ResponseMessage = $"An internal server error occurred.";
                }
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
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

            try
            {
                getCostCenter.Name = model.Name;
                getCostCenter.UpdatedBy = user.FullName;
                getCostCenter.UpdatedOn = DateTime.Now;

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    request.ResponseCode = 200;
                    request.ResponseMessage = $"Cost center has been modified.";
                    return await Task.Run(() => request);
                }
                else
                {
                    request.ResponseCode = 500;
                    request.ResponseMessage = $"An internal server error occurred.";
                }
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
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

            try
            {
                findCostCenter.IsActive = false;
                findCostCenter.UpdatedBy = user.FullName;
                findCostCenter.UpdatedOn = DateTime.Now;

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    request.ResponseCode = 200;
                    request.ResponseMessage = $"Cost center has been removed.";
                    return await Task.Run(() => request);
                }
                else
                {
                    request.ResponseCode = 500;
                    request.ResponseMessage = $"An internal server error occurred.";
                }
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }

            
            return await Task.Run(() => request);
        }

        #endregion

    }
}
