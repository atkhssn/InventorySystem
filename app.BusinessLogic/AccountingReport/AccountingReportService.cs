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

        public async Task<AccountingReportViewModel> GetTrialBalanceReportAsync()
        {
            var request = new AccountingReportViewModel();

            request.accountingReportViewModels = await _dbContext.Transactions
                .Include(v => v.Vouchers)
                .Include(ac => ac.ChartOfAccounts)
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
