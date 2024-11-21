using app.EntityModel.CoreModel;
using app.Infrastructure.Auth;
using app.Infrastructure.Repository;
using app.Infrastructure;
using app.Services.Voucher_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.EntityModel.DatabaseView;

namespace app.Services.Report_service
{
    public class ReportService : IReportService
    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;

        public ReportService(inventoryDbContext dbContext, IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }

        public async Task<VoucherViewModel> Generalledger(long vendorId)
        {
            VoucherViewModel model = new VoucherViewModel();
            var user = await workContext.GetCurrentUserAsync();
            var vendor = dbContext.Vendor.FirstOrDefault(d => d.Id == vendorId);
            model.VendorId = vendorId;
            model.VendorName = vendor.Name;
            model.VendorMobile = vendor.Mobile;
            model.VendorAddress = vendor.Address;
            model.VendorEmail = vendor.Email;
            model.UserAddress = user.Address;
            model.ImageUrl = user.ImageUrl;
            model.UserEmail = user.Email;
            model.UserMobile = user.PhoneNumber;
            model.ImageUrl = user.ImageUrl;
            var voucherList = await dbContext.Voucher.Where(f => f.VendorId == vendorId && f.TrakingId == user.Id && f.IsSubmitted == true && f.IsActive == true).ToListAsync();
            List<VoucherViewModel> vms = new List<VoucherViewModel>();
            decimal sumAmount = 0;
            foreach (var voucher in voucherList)
            {
                VoucherViewModel voucherView = new VoucherViewModel();
                voucherView.Id = voucher.Id;
                voucherView.VendorId = vendorId;
                voucherView.VoucherNo = voucher.VoucherNo;
                voucherView.VoucherDate = voucher.VoucherDate;
                voucherView.Narration = voucher.Narration;
                voucherView.PaymentMethod = voucher.PaymentMethod;
                if (voucher.VoucherTypeId == 10)
                {
                    voucherView.ReturnAmount = dbContext.VoucherDetails.Where(d => d.VoucherId == voucher.Id && d.IsActive == true&& d.DebitAmount > 0).Sum(d => d.DebitAmount);
                    voucherView.CreditAmount = 0;
                    voucherView.DebitAmount = 0;
                }
                else
                {
                    voucherView.ReturnAmount = 0;
                    voucherView.CreditAmount = dbContext.VoucherDetails.Where(d => d.VoucherId == voucher.Id && d.IsActive == true && d.CreditAmount > 0).Sum(d => d.CreditAmount);
                    voucherView.DebitAmount = dbContext.VoucherDetails.Where(d => d.VoucherId == voucher.Id && d.IsActive == true && d.DebitAmount > 0).Sum(d => d.DebitAmount);
                }

                sumAmount = sumAmount + voucherView.DebitAmount;
                sumAmount = sumAmount - voucherView.CreditAmount;
                sumAmount = sumAmount - voucherView.ReturnAmount;
                voucherView.Blance = sumAmount;
                vms.Add(voucherView);
            }
            model.datalist = vms;
            return model;
        }

        public async Task<IEnumerable<ProfitandLossStatement>> ProfitLoss(ReportsViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            model.TrakingId = user.Id;
            var StartDate = model.StartDate.ToString("dd/MM/yyyy");
            var EndDate = model.EndDate.ToString("dd/MM/yyyy");
            IQueryable<ProfitandLossStatement> list = dbContext.ProfitandLossStatement.FromSqlRaw("EXEC SP_Profit_Loss {0},{1},{2}",StartDate, EndDate, user.Id).AsQueryable();
            return list;    
        }

        public async Task<IEnumerable<PurchesViewRepot>> PurchesReport(ReportsViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            model.TrakingId = user.Id;
            var StartDate = model.StartDate.ToString("dd/MM/yyyy");
            var EndDate = model.EndDate.ToString("dd/MM/yyyy");
            if (model.SupplierId == null) { model.SupplierId = 0; }
            if (model.ProductId == null) { model.ProductId = 0; }
            if (model.CompanyId == null) { model.CompanyId = 0; }
            IQueryable<PurchesViewRepot> list = dbContext.PurchesViewRepot.FromSqlRaw("EXEC dbo.SP_PurchesReport {0},{1},{2},{3},{4},{5}", user.Id, StartDate, EndDate, model.SupplierId, model.CompanyId, model.ProductId).AsQueryable();
            return list;
        }

        public async Task<IEnumerable<SalesViewReport>> SalesReport(ReportsViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            model.TrakingId = user.Id;
            var StartDate = model.StartDate.ToString("dd/MM/yyyy");
            var EndDate = model.EndDate.ToString("dd/MM/yyyy");
            if (model.CustomerId == null) { model.CustomerId = 0; }
            if (model.ProductId == null) { model.ProductId = 0; }
            if (model.CompanyId == null) { model.CompanyId = 0; }
            IQueryable<SalesViewReport> list = dbContext.SalesViewReport.FromSqlRaw("EXEC dbo.SP_SaleReport {0},{1},{2},{3},{4},{5}", user.Id, StartDate, EndDate, model.CustomerId, model.CompanyId, model.ProductId).AsQueryable();
            return list;
        }
    }
}
