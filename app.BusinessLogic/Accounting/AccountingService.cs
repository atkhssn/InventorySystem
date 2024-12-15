using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace app.Services.Accounting
{
    public class AccountingService : IAccountingService
    {
        private readonly inventoryDbContext _dbContext;
        private readonly IWorkContext _workContext;
        private readonly string _allowedAccountCode = @"^[0-9]*$";
        public AccountingService(IWorkContext workContext, inventoryDbContext dbContext)
        {
            _dbContext = dbContext;
            _workContext = workContext;
        }

        #region Chart of Accounting

        public async Task<ChartOfAccountsViewModel> ChartOfAccoutingsAsync()
        {
            var request = new ChartOfAccountsViewModel();
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
            var request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var fetchCoAList = await _dbContext.ChartOfAccounts.Where(x => x.IsActive).ToListAsync();
            var findParentHead = fetchCoAList.Where(x => x.AccountCode.Equals(model.ParentAccountCode)).FirstOrDefault();
            string accountCode = string.Empty;

            if (!Regex.IsMatch(model.AccountCode, _allowedAccountCode))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Invalid account code: {model.AccountCode}";
                return request;
            }

            if (findParentHead.Level.Equals(1))
            {
                if (model.AccountCode.Length.Equals(2))
                {
                    accountCode = $"{findParentHead.AccountCode}0{model.AccountCode}";
                }
                else
                {
                    request.ResponseCode = 400;
                    request.ResponseMessage = "Minimum or Maximum account code digit: 2";
                    return request;
                }
            }
            else if (findParentHead.Level.Equals(2))
            {
                if (model.AccountCode.Length.Equals(3))
                {
                    accountCode = $"{findParentHead.AccountCode}0{model.AccountCode}";
                }
                else
                {
                    request.ResponseCode = 400;
                    request.ResponseMessage = "Minimum or Maximum account code digit: 3";
                    return request;
                }
            }
            else if (findParentHead.Level.Equals(3))
            {
                if (model.AccountCode.Length.Equals(2))
                {
                    accountCode = $"{findParentHead.AccountCode}0{model.AccountCode}";
                }
                else
                {
                    request.ResponseCode = 400;
                    request.ResponseMessage = "Minimum or Maximum account code digit: 2";
                    return request;
                }
            }
            else if (findParentHead.Level.Equals(4))
            {
                if (model.AccountCode.Length.Equals(1))
                {
                    accountCode = $"{findParentHead.AccountCode}0{model.AccountCode}";
                }
                else
                {
                    request.ResponseCode = 400;
                    request.ResponseMessage = "Minimum or Maximum account code digit: 1";
                    return request;
                }
            }

            if (findParentHead.Level.Equals(5))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = "Cannot create new head under the GL HEAD.";
                return request;
            }

            if (findParentHead is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"Parent code [{model.ParentAccountCode}] is not found.";
                return request;
            }

            if (fetchCoAList.Any(x => x.ParentAccountCode.Equals(findParentHead.AccountCode) && x.AccountCode.Equals(accountCode)))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.AccountCode}] has already been added.";
                return request;
            }

            if (fetchCoAList.Any(x => x.ParentAccountCode.Equals(findParentHead.AccountCode) && x.AccountName.ToUpper().Equals(model.AccountName.ToUpper())))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.AccountName}] has already been added.";
                return request;
            }

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                var chartOfAccounts = new ChartOfAccounts
                {
                    AccountCode = accountCode,
                    AccountName = model.AccountName,
                    ParentAccountCode = model.ParentAccountCode,
                    Level = findParentHead.Level + 1,
                    CoATypeId = findParentHead.CoATypeId,
                    CreatedBy = user.FullName,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };

                _dbContext.ChartOfAccounts.Add(chartOfAccounts);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                request.ResponseCode = 200;
                request.ResponseMessage = "New account head has been added.";
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();
            }
            return request;
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
                request.ResponseMessage = "No records found to update.";
                return request;
            }

            if (fetchCoAList.Where(x => x.ParentAccountCode.Equals(findHead.ParentAccountCode) && x.AccountName.Equals(model.AccountName)).FirstOrDefault() is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.AccountName}] has already been used.";
                return request;
            }

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                findHead.AccountName = model.AccountName;
                findHead.UpdatedBy = user.FullName;
                findHead.UpdatedOn = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                request.ResponseCode = 200;
                request.ResponseMessage = "Account head has been modified.";
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
            return request;
        }

        public async Task<ResponseViewModel> DeleteAccountHeadAsync(string accountCode)
        {
            var request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findHead = await _dbContext.ChartOfAccounts
                .FirstOrDefaultAsync(x => x.IsActive && x.AccountCode.Equals(accountCode));

            if (findHead is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to delete.";
                return request;
            }

            if (findHead.ParentAccountCode.Equals("0"))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = "Account first head is not deletable.";
                return request;
            }

            if (await IsInVoucher(accountCode))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Cannot delete, [{accountCode}] is used in voucher.";
                return request;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var accountsToDelete = await GetDescendantAccountsAsync(accountCode);

                foreach (var account in accountsToDelete)
                {
                    if (await IsInVoucher(account.AccountCode))
                    {
                        request.ResponseCode = 400;
                        request.ResponseMessage = $"Cannot delete, [{account.AccountCode}] is used in voucher.";
                        return request;
                    }
                    else
                    {
                        account.IsActive = false;
                        account.UpdatedBy = user.FullName;
                        account.UpdatedOn = DateTime.Now;
                    }
                }

                findHead.IsActive = false;
                findHead.UpdatedBy = user.FullName;
                findHead.UpdatedOn = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                request.ResponseCode = 200;
                request.ResponseMessage = "Account head has been removed.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();
            }
            return request;
        }

        //Check account code in voucher
        private async Task<bool> IsInVoucher(string accountCode)
        {
            return await _dbContext.VouchersLines.AnyAsync(x => x.AccountCode.Equals(accountCode) && x.IsActive);
        }

        //Get Descendant Account
        private async Task<List<ChartOfAccounts>> GetDescendantAccountsAsync(string accountCode)
        {
            var accountsToDelete = new List<ChartOfAccounts>();
            var queue = new Queue<string>();
            queue.Enqueue(accountCode);

            while (queue.Count > 0)
            {
                var parentCode = queue.Dequeue();
                var childAccounts = await _dbContext.ChartOfAccounts
                    .Where(x => x.IsActive && x.ParentAccountCode.Equals(parentCode))
                    .ToListAsync();

                accountsToDelete.AddRange(childAccounts);
                foreach (var child in childAccounts)
                {
                    queue.Enqueue(child.AccountCode);
                }
            }
            return accountsToDelete;
        }

        //Autocomplete
        public async Task<List<ChartOfAccountHierarchy>> GetGLAcoountHeadAsync()
        {
            var request = await _dbContext.ChartOfAccounts
                .Where(x => x.Level.Equals(5) && x.IsActive)
                .Select(x => new ChartOfAccountHierarchy
                {
                    id = x.AccountCode,
                    text = $"[{x.AccountCode}]-{x.AccountName}"
                }).ToListAsync();

            return request;
        }

        #endregion

        #region Cost Center

        public async Task<CostCentersViewModel> CostCentersAsync()
        {
            var request = new CostCentersViewModel();
            request.CostCenterViewModels = await _dbContext.CostCenters
                .Where(x => x.IsActive)
                .Select(x => new CostCentersViewModel
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
            var request = new CostCentersViewModel();
            request = await _dbContext.CostCenters
                .Where(x => x.Id.Equals(id) && x.IsActive)
                .Select(x => new CostCentersViewModel
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
            var request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Name.ToUpper().Equals(model.Name.ToUpper())).FirstOrDefault();
            var findCostCenterShort = _dbContext.CostCenters.Where(x => x.ShortName.ToUpper().Equals(model.ShortName.ToUpper())).FirstOrDefault();

            if (findCostCenter is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.Name}] has already been added.";
                return request;
            }

            if (findCostCenterShort is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.ShortName}] has already been added.";
                return request;
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

                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                _dbContext.CostCenters.Add(costCenter);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                request.ResponseCode = 200;
                request.ResponseMessage = "New cost center has been added.";
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
            return request;
        }

        public async Task<ResponseViewModel> UpdateCostCenterAync(CostCentersViewModel model)
        {
            var request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var getCostCenter = _dbContext.CostCenters.Where(x => x.Id.Equals(model.Id) && x.ShortName.ToUpper().Equals(model.ShortName.ToUpper())).FirstOrDefault();
            var findCostCenter = _dbContext.CostCenters.Where(x => x.Name.ToUpper().Equals(model.Name.ToUpper())).FirstOrDefault();

            if (getCostCenter is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = "No records found to update.";
                return request;
            }

            if (findCostCenter is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.Name}] has already been used.";
                return request;
            }

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                getCostCenter.Name = model.Name;
                getCostCenter.UpdatedBy = user.FullName;
                getCostCenter.UpdatedOn = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                request.ResponseCode = 200;
                request.ResponseMessage = "Cost center has been modified.";
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();

            }
            return request;
        }

        public async Task<ResponseViewModel> DeleteCostCenterAync(long id)
        {
            var request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findCostCenter = await _dbContext.CostCenters.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            var isInVoucher = await _dbContext.Vouchers.AnyAsync(x => x.CostCentersId.Equals(id) && x.IsActive);


            if (findCostCenter is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = "No records found to delete.";
                return request;
            }

            if (isInVoucher)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = "Cannot delete, Already used in voucher.";
                return request;
            }

            try
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                findCostCenter.IsActive = false;
                findCostCenter.UpdatedBy = user.FullName;
                findCostCenter.UpdatedOn = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                request.ResponseCode = 200;
                request.ResponseMessage = "Cost center has been removed.";
            }
            catch (Exception ex)
            {
                request.ResponseCode = 500;
                request.ResponseMessage = ex.Message.ToString();
            }
            return request;
        }

        #endregion
    }
}
