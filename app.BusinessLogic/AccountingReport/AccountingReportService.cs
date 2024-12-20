using app.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace app.Services.AccountingReport
{
    public class AccountingReportService : IAccountingReportService
    {
        private readonly inventoryDbContext _dbContext;

        public AccountingReportService(inventoryDbContext inventoryDbContext)
        {
            _dbContext = inventoryDbContext;
        }

        public async Task<AccountingReportViewModel> GetTrialBalanceReportAsync(AccountingReportViewModel model)
        {
            var request = new AccountingReportViewModel();
            var query = _dbContext.Transactions
                .Include(vo => vo.Vouchers)
                .Include(cc => cc.Vouchers.CostCenters)
                .Include(ca => ca.ChartOfAccounts)
                .AsQueryable();

            if (model.CostCentersId.HasValue && model.CostCentersId > 0)
            {
                query = query.Where(vo => vo.Vouchers.CostCenters.Id.Equals(model.CostCentersId));
            }

            if (model.FromDate.HasValue)
            {
                query = query.Where(fd => fd.TransactionDate.Date >= model.FromDate.Value.Date);
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(td => td.TransactionDate.Date <= model.ToDate.Value.Date);
            }


            request.accountingReportViewModels = await query
                .Select(x => new AccountingReportViewModel
                {
                    TransactionId = x.Id,
                    VoucherNo = x.Vouchers.VoucherNo,
                    AccountCode = x.AccountCode,
                    AccountName = x.ChartOfAccounts.AccountName,
                    DebitAmount = x.DebitAmount,
                    CreditAmount = x.CreditAmount,
                    TransactionDate = x.TransactionDate,
                }).OrderByDescending(x => x.TransactionDate).ToListAsync();

            request.TotalDebitAmount = request.accountingReportViewModels.Sum(x => x.DebitAmount);
            request.TotalCreditAmount = request.accountingReportViewModels.Sum(x => x.CreditAmount);

            return request;
        }
    }
}
