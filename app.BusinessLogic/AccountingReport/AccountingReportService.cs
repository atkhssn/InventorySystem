﻿using app.Infrastructure;
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
                .ThenInclude(cc => cc.CostCenters)
                .Include(ca => ca.ChartOfAccounts)
                .AsQueryable();

            if (model.CostCentersId.HasValue && model.CostCentersId > 0)
            {
                query = query.Where(vo => vo.Vouchers.CostCenters.Id.Equals(model.CostCentersId));
            }

            if (model.FromDate.HasValue)
            {
                query = query.Where(fd => fd.TransactionDate.Date >= model.FromDate.Value.Date);
                request.FromDate = model.FromDate;
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(td => td.TransactionDate.Date <= model.ToDate.Value.Date);
                request.ToDate = model.ToDate;
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

        public async Task<AccountingReportViewModel> GetReceivableReportAsync(AccountingReportViewModel model)
        {
            var request = new AccountingReportViewModel();
            const string AccountReceivableHead = "1011012"; //3rd layer head code
            var fetch4thHeads = await _dbContext.ChartOfAccounts
                .Where(x => x.ParentAccountCode.Equals(AccountReceivableHead) && x.IsActive)
                .Select(x => x.AccountCode)
                .ToListAsync();

            var fetch5thHeads = await _dbContext.ChartOfAccounts
                .Where(x => fetch4thHeads.Contains(x.ParentAccountCode) && x.IsActive)
                .Select(x => x.AccountCode)
                .ToListAsync();

            var query = _dbContext.Transactions
                .Include(vo => vo.Vouchers)
                .ThenInclude(cc => cc.CostCenters)
                .Where(tr => fetch5thHeads.Contains(tr.AccountCode))
                .AsQueryable();

            if (model.SearchAccountCode is not null && !model.SearchAccountCode.Equals("0"))
            {
                query = query.Where(ca => ca.AccountCode.Equals(model.SearchAccountCode));
                request.AccountCode = model.SearchAccountCode;
            }

            if (model.FromDate.HasValue)
            {
                query = query.Where(fd => fd.TransactionDate.Date >= model.FromDate.Value.Date);
                request.FromDate = model.FromDate;
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(td => td.TransactionDate.Date <= model.ToDate.Value.Date);
                request.ToDate = model.ToDate;
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

        public async Task<AccountingReportViewModel> GetPayableReportAsync(AccountingReportViewModel model)
        {
            var request = new AccountingReportViewModel();
            const string AccountPayableHead = "2011011"; //3rd layer head code
            var fetch4thHeads = await _dbContext.ChartOfAccounts
                .Where(x => x.ParentAccountCode.Equals(AccountPayableHead) && x.IsActive)
                .Select(x => x.AccountCode)
                .ToListAsync();

            var fetch5thHeads = await _dbContext.ChartOfAccounts
                .Where(x => fetch4thHeads.Contains(x.ParentAccountCode) && x.IsActive)
                .Select(x => x.AccountCode)
                .ToListAsync();

            var query = _dbContext.Transactions
                .Include(vo => vo.Vouchers)
                .ThenInclude(cc => cc.CostCenters)
                .Where(tr => fetch5thHeads.Contains(tr.AccountCode))
                .AsQueryable();

            if (model.SearchAccountCode is not null && !model.SearchAccountCode.Equals("0"))
            {
                query = query.Where(ca => ca.AccountCode.Equals(model.SearchAccountCode));
                request.AccountCode = model.SearchAccountCode;
            }

            if (model.FromDate.HasValue)
            {
                query = query.Where(fd => fd.TransactionDate.Date >= model.FromDate.Value.Date);
                request.FromDate = model.FromDate;
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(td => td.TransactionDate.Date <= model.ToDate.Value.Date);
                request.ToDate = model.ToDate;
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
