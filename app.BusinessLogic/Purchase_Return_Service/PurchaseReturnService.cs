using app.Infrastructure.Auth;
using app.Infrastructure;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.EntityModel.CoreModel;
using app.Services.SalaesReturn_service;

namespace app.Services.Purchase_Return_Service
{
    public class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public PurchaseReturnService(inventoryDbContext dbContext,
            IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<long> AddPurchaseReturn(PurchaseReturnViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var poMax = await dbContext.PurchaseReturn.Where(x => x.TrakingId == user.Id).CountAsync() + 1;
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var poCid = @"PR-" +
                    DateTime.Now.ToString("yy") + "-" +
                    DateTime.Now.ToString("MM") + "-" +
                    DateTime.Now.ToString("dd") + "-" +
                    poMax.ToString().PadLeft(2, '0');
                    PurchaseReturn order = new PurchaseReturn();
                    order.PurchaseReturnNo = poCid;
                    order.SupplierId = model.SupplierId;
                    order.PurchaseReturnDate = model.PurchaseReturnDate;
                    order.Reason = model.Reason;
                    order.CreatedBy = user.FullName;
                    order.TrakingId = user.Id;
                    order.CreatedOn = BaTime;
                    order.IsActive = true;
                    dbContext.PurchaseReturn.Add(order);
                    dbContext.SaveChanges();
                    List<PurchaseReturnDetails> purchesReturnDetailsDetails = new List<PurchaseReturnDetails>();
                    foreach (var item in model.MappVm)
                    {
                        PurchaseReturnDetails details = new PurchaseReturnDetails();
                        details.PurchaseReturnId = order.Id;
                        details.ProductId = item.ProductId;
                        details.ReturnQty = item.ReturnQty;
                        details.PurchaseReturnRate = item.PurchaseReturnRate;
                        details.PurchaseReturnAmount = item.PurchaseReturnAmount;
                        details.UnitName = item.UnitName;
                        details.CreatedBy = user.FullName;
                        details.CreatedOn = BaTime;
                        details.TrakingId = user.Id;
                        details.IsActive = true;
                        purchesReturnDetailsDetails.Add(details);
                    }
                    dbContext.PurchaseReturnDetails.AddRange(purchesReturnDetailsDetails);
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

        public async Task<long> AddPurchaseReturnDetalies(PurchaseReturnDetailsViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            PurchaseReturnDetails details = new PurchaseReturnDetails();
            details.PurchaseReturnId = model.PurchaseReturnId;
            details.ProductId = model.ProductId;
            details.ReturnQty = model.ReturnQty;
            details.PurchaseReturnRate = model.PurchaseReturnRate;
            details.PurchaseReturnAmount = model.PurchaseReturnAmount;
            details.UnitName = model.UnitName;
            details.CreatedBy = user.FullName;
            details.CreatedOn = BaTime;
            details.TrakingId = user.Id;
            details.IsActive = true;
            dbContext.PurchaseReturnDetails.Add(details);
            dbContext.SaveChanges();
            return model.PurchaseReturnId;
        }

        public Task<long> DeletePurchaseReturn(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<long> DeletePurchaseReturnDetalies(long id)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var result = await dbContext.PurchaseReturnDetails.FirstOrDefaultAsync(x => x.Id == id);
            result.IsActive = false;
            result.UpdatedOn = BaTime;
            result.CreatedBy = user.FullName;
            dbContext.Entry(result).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return result.PurchaseReturnId;
        }

        public async Task<PagedModel<PurchaseReturnViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString)
        {
            PurchaseReturnViewModel model = new PurchaseReturnViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.datalist = await Task.Run(() => (from t1 in dbContext.PurchaseReturn
                                                   join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                   join t3 in dbContext.Vendor on t1.SupplierId equals t3.Id
                                                   where t1.IsActive == true && t1.TrakingId == user.Id
                                                   select new PurchaseReturnViewModel
                                                   {
                                                       Id = t1.Id,
                                                       Reason = t1.Reason,
                                                       TrakingId=t1.TrakingId,
                                                       SupplierName = t3.Name == null ? "" : t3.Name,
                                                       PurchaseReturnNo = t1.PurchaseReturnNo,
                                                       PurchaseReturnDate = t1.PurchaseReturnDate,
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
                    t.SupplierName.ToLower().Contains(sarchString) ||
                    t.PurchaseReturnNo.ToLower().Contains(sarchString)
                );
            }
            int resCount = model.datalist.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.datalist.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<PurchaseReturnViewModel> pagedModel = new PagedModel<PurchaseReturnViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<PurchaseReturnViewModel> GetPurchaseReturn(long id)
        {
            PurchaseReturnViewModel model = new PurchaseReturnViewModel();

            var user = await workContext.GetCurrentUserAsync();
            model = await Task.Run(() => (from t1 in dbContext.PurchaseReturn
                                          join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                          join vendor in dbContext.Vendor on t1.SupplierId equals vendor.Id
                                          where t1.IsActive == true && t1.Id == id
                                          select new PurchaseReturnViewModel
                                          {
                                              Id = t1.Id,
                                              UserName = t2.FullName,
                                              UserEmail = t2.Email,
                                              UserMobile = t2.PhoneNumber,
                                              ImageUrl = t2.ImageUrl,
                                              UserAddress = t2.Address,
                                              TrakingId = t1.TrakingId,
                                              SupplierName = vendor.Name,
                                              SupplierId = t1.SupplierId,
                                              SupplierMobile = vendor.Mobile,
                                              SupplierAddress = vendor.Address,
                                              PurchaseReturnNo = t1.PurchaseReturnNo,
                                              PurchaseReturnDate = t1.PurchaseReturnDate,
                                              Reason = t1.Reason,
                                              IsSubmited = t1.IsSubmited,
                                              CreatedBy = t1.CreatedBy,
                                              CreatedOn = t1.CreatedOn,
                                          }).FirstOrDefaultAsync());

            model.MappVm = await Task.Run(() => (from t1 in dbContext.PurchaseReturnDetails
                                                 join t2 in dbContext.Product on t1.ProductId equals t2.Id
                                                 join t3 in dbContext.ProductSubCategory on t2.ProductSubCategoryId equals t3.Id
                                                 join t4 in dbContext.ProductCategory on t2.ProductCategoryId equals t4.Id
                                                 where t1.IsActive == true && t1.PurchaseReturnId == model.Id
                                                 select new PurchaseReturnDetailsViewModel
                                                 {
                                                     Id = t1.Id,
                                                     ProductId = t1.ProductId,
                                                     PurchaseReturnAmount = t1.PurchaseReturnAmount,
                                                     ReturnQty = t1.ReturnQty,
                                                     PurchaseReturnRate = t1.PurchaseReturnRate,
                                                     UnitName = t1.UnitName,
                                                     PurchaseReturnId = t1.PurchaseReturnId,
                                                     ProductName = t4.Name + "-" + t3.Name + "-" + t2.ProductName
                                                 }).ToListAsync());
            return model;
        }

        public async Task<long> UpdatePurchaseReturn(PurchaseReturnViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            PurchaseReturn order = await dbContext.PurchaseReturn.FirstOrDefaultAsync(d => d.Id == model.Id);
            order.SupplierId = model.SupplierId;
            order.PurchaseReturnDate = model.PurchaseReturnDate;
            order.Reason = model.Reason;
            order.UpdatedBy = user.FullName;
            order.UpdatedOn = BaTime;
            dbContext.Entry(order).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return order.Id;
        }
    }
}
