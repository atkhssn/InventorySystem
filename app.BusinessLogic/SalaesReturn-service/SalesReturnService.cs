using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using app.Services.PurchaseOrder_Services;
using app.Utility.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services.SalaesReturn_service
{
    public class SalesReturnService : ISalesReturns

    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public SalesReturnService(inventoryDbContext dbContext,
            IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<long> AddSalesReturn(SalesReturnViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var poMax = await dbContext.SalesReturn.Where(x => x.TrakingId == user.Id).CountAsync() + 1;
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var poCid = @"SR-" +
                    DateTime.Now.ToString("yy") + "-" +
                    DateTime.Now.ToString("MM") + "-" +
                    DateTime.Now.ToString("dd") + "-" +
                    poMax.ToString().PadLeft(2, '0');
                    SalesReturn order = new SalesReturn();
                    order.SalesReturnNo = poCid;
                    order.CustomerId = model.CustomerId;
                    order.SalesReturnDate = model.SalesReturnDate;
                    order.Reason = model.Reason;
                    order.CreatedBy = user.FullName;
                    order.TrakingId = user.Id;
                    order.CreatedOn = BaTime;
                    order.IsActive = true;
                    dbContext.SalesReturn.Add(order);
                    dbContext.SaveChanges();
                    List<SalesReturnDetails> salesReturnDetailsDetails = new List<SalesReturnDetails>();
                    foreach (var item in model.MappVm)
                    {
                        SalesReturnDetails details = new SalesReturnDetails();
                        details.SalesReturnId = order.Id;
                        details.ProductId = item.ProductId;
                        details.ReturnQty = item.ReturnQty;
                        details.ReturnRate = item.ReturnRate;
                        details.ReturnAmount = item.ReturnAmount;
                        details.UnitName = item.UnitName;
                        details.CreatedBy = user.FullName;
                        details.CreatedOn = BaTime;
                        details.TrakingId = user.Id;
                        details.IsActive = true;
                        salesReturnDetailsDetails.Add(details);
                    }
                    dbContext.SalesReturnDetails.AddRange(salesReturnDetailsDetails);
                    dbContext.SaveChanges();
                    scope.Commit();
                    return order.Id;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return 0;
                }
            }

        }

        public async Task<long> AddSalesReturnDetalies(SalesReturnDetailsViewmodel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            SalesReturnDetails details = new SalesReturnDetails();
            details.SalesReturnId =model.SalesReturnId;
            details.ProductId = model.ProductId;
            details.ReturnQty = model.ReturnQty;
            details.ReturnRate = model.ReturnRate;
            details.ReturnAmount = model.ReturnAmount;
            details.UnitName = model.UnitName;
            details.CreatedBy = user.FullName;
            details.CreatedOn = BaTime;
            details.TrakingId = user.Id;
            details.IsActive = true;
            dbContext.SalesReturnDetails.Add(details);
            dbContext.SaveChanges();
            return model.SalesReturnId; 
        }

        public async Task<long> DeleteSalesReturnDetalies(long id)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var result = await dbContext.SalesReturnDetails.FirstOrDefaultAsync(x => x.Id == id);
            result.IsActive = false;
            result.UpdatedOn = BaTime;
            result.CreatedBy = user.FullName;
            dbContext.Entry(result).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return result.SalesReturnId;
        }

        public async Task<PagedModel<SalesReturnViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString)
        {
            SalesReturnViewModel model = new SalesReturnViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.datalist = await Task.Run(() => (from t1 in dbContext.SalesReturn
                                                   join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                   join t3 in dbContext.Vendor on t1.CustomerId equals t3.Id
                                                   where t1.IsActive == true && t1.TrakingId==user.Id
                                                   select new SalesReturnViewModel
                                                   {
                                                       Id = t1.Id,
                                                       UserName = t2.FullName,
                                                       TrakingId = t1.TrakingId,
                                                       CustomerName = t3.Name==null?"":t3.Name,
                                                       SalesReturnNo=t1.SalesReturnNo,
                                                       SalesReturnDate = t1.SalesReturnDate,
                                                       Reason = t1.Reason,    
                                                       IsSubmited = t1.IsSubmited
                                                   }).OrderBy(d => d.IsSubmited).AsQueryable());
            if (user.UserType == 2)
            {
                model.datalist = model.datalist.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.datalist = model.datalist.Where(t =>
                    t.CustomerName.ToLower().Contains(sarchString) ||
                    t.SalesReturnNo.ToLower().Contains(sarchString) 
                );
            }
            int resCount = model.datalist.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.datalist.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<SalesReturnViewModel> pagedModel = new PagedModel<SalesReturnViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<SalesReturnViewModel> GetSalesReturn(long id)
        {
            SalesReturnViewModel model = new SalesReturnViewModel();

            var user = await workContext.GetCurrentUserAsync();
            model = await Task.Run(() => (from t1 in dbContext.SalesReturn
                                          join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                          join vendor in dbContext.Vendor on t1.CustomerId equals vendor.Id
                                          where t1.IsActive == true && t1.Id == id
                                          select new SalesReturnViewModel
                                          {
                                              Id = t1.Id,
                                              UserName = t2.FullName,
                                              ImageUrl = t2.ImageUrl,
                                              UserEmail = t2.Email,
                                              UserMobile = t2.PhoneNumber,
                                              UserAddress = t2.Address,
                                              TrakingId = t1.TrakingId,
                                              CustomerName = vendor.Name,
                                              CustomerId = t1.CustomerId,
                                              CustomerMobile = vendor.Mobile,
                                              CustomerAddress = vendor.Address,
                                              SalesReturnNo = t1.SalesReturnNo,
                                              SalesReturnDate = t1.SalesReturnDate,
                                              Reason = t1.Reason,
                                              IsSubmited = t1.IsSubmited,
                                              CreatedBy = t1.CreatedBy,
                                              CreatedOn = t1.CreatedOn, 
                                                }).FirstOrDefaultAsync());

            model.MappVm = await Task.Run(() => (from t1 in dbContext.SalesReturnDetails
                                                 join t2 in dbContext.Product on t1.ProductId equals t2.Id
                                                 join t3 in dbContext.ProductSubCategory on t2.ProductSubCategoryId equals t3.Id
                                                 join t4 in dbContext.ProductCategory on t2.ProductCategoryId equals t4.Id
                                                 where t1.IsActive == true && t1.SalesReturnId == model.Id
                                                 select new SalesReturnDetailsViewmodel
                                                 {
                                                     Id = t1.Id,
                                                     ProductId = t1.ProductId,
                                                     ReturnAmount = t1.ReturnAmount,
                                                     ReturnQty = t1.ReturnQty,
                                                     ReturnRate = t1.ReturnRate,
                                                     UnitName = t1.UnitName,
                                                     SalesReturnId = t1.SalesReturnId,
                                                     ProductName = t4.Name + "-" + t3.Name + "-" + t2.ProductName
                                                 }).ToListAsync());
            return model;
        }

        public async Task<long> UpdateSalesReturn(SalesReturnViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            SalesReturn order = await dbContext.SalesReturn.FirstOrDefaultAsync(d => d.Id == model.Id);
            order.CustomerId = model.CustomerId;
            order.SalesReturnDate = model.SalesReturnDate;
            order.Reason = model.Reason;
            order.UpdatedBy = user.FullName;
            order.UpdatedOn = BaTime;
            dbContext.Entry(order).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return order.Id;
        }
    }
}
