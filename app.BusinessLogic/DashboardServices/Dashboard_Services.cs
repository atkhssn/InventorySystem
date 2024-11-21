using app.Infrastructure.Auth;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Services.Report_service;
using Microsoft.EntityFrameworkCore;
using app.EntityModel.CoreModel;
using app.Services.PurchaseOrder_Services;

namespace app.Services.DashboardServices
{
    public class Dashboard_Services : IDashboard_Services
    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public Dashboard_Services(inventoryDbContext dbContext,
            IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<DashbordViewModel> dashbordViewModel()
        {
            DashbordViewModel dashbordViewModel = new DashbordViewModel();
            var user = await workContext.GetCurrentUserAsync();
            var res=await dbContext.Vendor.Where(f=>f.IsActive==true&&f.TrakingId== user.Id).ToListAsync();  
            dashbordViewModel.CoustomerCount = res.Where(f=>f.VendorType==2).Count();  
            dashbordViewModel.SupplierCount = res.Where(f=>f.VendorType==1).Count();  
            dashbordViewModel.UserType = user.UserType;
            var purches = await dbContext.Voucher.Where(f => f.VoucherDate.Date == DateTime.Now.Date && f.TrakingId == user.Id && f.VoucherTypeId == 7).ToListAsync();           
            dashbordViewModel.PurchaseOrderCount = purches.Count();
            var model = await Task.Run(() => (from t1 in dbContext.Voucher
                                              join t2 in dbContext.VoucherDetails on t1.Id equals t2.VoucherId
                                              where t1.IsActive == true && t1.TrakingId == user.Id &&t1.VoucherDate.Date == DateTime.Now.Date 
                                              &&t1.VoucherTypeId== 7  
                                              select new PurchaseOrderViewModel
                                              {
                                                  Amount = t2.DebitAmount
                                              }).ToListAsync());

            dashbordViewModel.PurchaseOrderAmt = model.Sum(f => f.Amount);
            var salse = await dbContext.Voucher.Where(f => f.VoucherDate.Date == DateTime.Now.Date && f.TrakingId == user.Id && f.VoucherTypeId ==6).ToListAsync();
            dashbordViewModel.SalesCount = salse.Count();
            var svmodel = await Task.Run(() => (from t1 in dbContext.Voucher
                                              join t2 in dbContext.VoucherDetails on t1.Id equals t2.VoucherId
                                              where t1.IsActive == true && t1.TrakingId == user.Id && t1.VoucherDate.Date == DateTime.Now.Date
                                              && t1.VoucherTypeId ==6
                                              select new PurchaseOrderViewModel
                                              {
                                                  Amount = t2.DebitAmount
                                              }).ToListAsync());
            dashbordViewModel.SalesAmt = svmodel.Sum(f => f.Amount);


           
            var collacttion = await Task.Run(() => (from t1 in dbContext.Voucher
                                                join t2 in dbContext.VoucherDetails on t1.Id equals t2.VoucherId
                                                join t3 in dbContext.Vendor on t1.VendorId equals t3.Id
                                                where t1.IsActive == true && t1.TrakingId == user.Id && t1.VoucherDate.Date == DateTime.Now.Date
                                                && t3.VendorType == 2
                                                select new PurchaseOrderViewModel
                                                {
                                                    Amount = t2.CreditAmount
                                                }).ToListAsync());
            dashbordViewModel.CollactionAmt = collacttion.Sum(f => f.Amount);
          
            var payment = await Task.Run(() => (from t1 in dbContext.Voucher
                                                 join t2 in dbContext.VoucherDetails on t1.Id equals t2.VoucherId
                                                 join t3 in dbContext.Vendor on t1.VendorId equals t3.Id
                                                 where t1.IsActive == true && t1.TrakingId == user.Id && t1.VoucherDate.Date == DateTime.Now.Date
                                                 && t3.VendorType == 1
                                                 select new PurchaseOrderViewModel
                                                 {
                                                     Amount = t2.CreditAmount
                                                 }).ToListAsync());
            dashbordViewModel.PaymentAmt = payment.Sum(f => f.Amount);
            return dashbordViewModel; 
        }
    }
}
