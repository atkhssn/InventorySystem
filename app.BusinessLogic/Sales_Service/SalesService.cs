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

namespace app.Services.Sales_Service
{
    public class SalesService : ISalesService
    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public SalesService(inventoryDbContext dbContext,
            IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }

        public async Task<long> AddSaleDetalies(SalesOrderDetailsViewModel item)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            SalesOrderDetails details = new SalesOrderDetails();
            details.SalesOrderId = item.SalesOrderId;
            details.ProductId = item.ProductId;
            details.SalesQty = item.SalesQty;
            details.SalesRate = item.SalesRate;
            details.SalesAmount = item.SalesAmount;
            details.PackSize = item.PackSize;
            details.CreatedBy = user.FullName;
            details.CreatedOn = BaTime;
            details.TrakingId = user.Id;
            dbContext.SalesOrderDetails.Add(details);
            dbContext.SaveChanges();
            return details.SalesOrderId;
        }


        public async Task<long> AddSalesOrder(SalesViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var poMax = await dbContext.SalesOrder.Where(x => x.TrakingId == user.Id).CountAsync() + 1;
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (model.CustomerId == 0)
                    {
                        Vendor vendor = new Vendor();
                        vendor.Name = model.CustomerName;
                        vendor.Mobile = model.CustomerMobile;
                        vendor.Email = model.CustomerEmail;
                        vendor.Address = model.CustomerAddress;
                        vendor.VendorType = 2;
                        vendor.CreatedBy = user.FullName;
                        vendor.TrakingId = user.Id;
                        vendor.CreatedOn = BaTime;
                        dbContext.Add(vendor);
                        dbContext.SaveChanges();
                        model.CustomerId = vendor.Id;
                    }

                    var poCid = @"SV-" +
                    DateTime.Now.ToString("yy") + "-" +
                    DateTime.Now.ToString("MM") + "-" +
                    DateTime.Now.ToString("dd") + "-" +
                    poMax.ToString().PadLeft(2, '0');
                    SalesOrder order = new SalesOrder();
                    order.SalesOrderNo = poCid;
                    order.CustomerId = model.CustomerId;
                    order.SalesDate = model.SalesDate;
                    order.DeliveryDate = model.DeliveryDate;
                    order.DeliveryAddress = model.DeliveryAddress;

                    order.OtherCharge = model.OtherCharge;
                    order.SupplierPaymentMethodEnumFK = model.SupplierPaymentMethodEnumFK;
                    order.Description = model.Description;
                    order.TermsAndCondition = model.TermsAndCondition;
                    order.Status = model.Status;
                    order.CreatedBy = user.FullName;
                    order.TrakingId = user.Id;
                    order.CreatedOn = BaTime;
                    order.DiscountAmount = 0;
                    order.Discountpercentages = 0;
                    dbContext.SalesOrder.Add(order);
                    dbContext.SaveChanges();
                    List<SalesOrderDetails> salesOrderDetails = new List<SalesOrderDetails>();
                    foreach (var item in model.MappVm)
                    {
                        SalesOrderDetails details = new SalesOrderDetails();
                        details.SalesOrderId = order.Id;
                        details.ProductId = item.ProductId;
                        details.SalesQty = item.SalesQty;
                        details.SalesRate = item.SalesRate;
                        details.SalesAmount = item.SalesAmount;
                        details.PackSize = item.PackSize;
                        details.CreatedBy = user.FullName;
                        details.CreatedOn = BaTime;
                        details.TrakingId = user.Id;
                        details.Discount_Persentage=item.Discount_Persentage;
                        details.Discount_Amount=item.Discount_Amount;
                        salesOrderDetails.Add(details);
                    }
                    dbContext.SalesOrderDetails.AddRange(salesOrderDetails);
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

        public Task<long> DeleteSale(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<long> DeleteSaleOrderDetalies(long id)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var result = await dbContext.SalesOrderDetails.FirstOrDefaultAsync(x => x.Id == id);
            result.IsActive = false;
            result.UpdatedOn = BaTime;
            result.CreatedBy = user.FullName;
            dbContext.Entry(result).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return result.SalesOrderId;

        }

        public async Task<PagedModel<SalesViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString)
        {
            SalesViewModel model = new SalesViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.datalist = await Task.Run(() => (from t1 in dbContext.SalesOrder
                                                   join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                   join t3 in dbContext.Vendor on t1.CustomerId equals t3.Id
                                                   where t1.IsActive == true && t1.TrakingId == user.Id 
                                                   select new SalesViewModel
                                                   {
                                                       Id = t1.Id,
                                                       UserName = t2.FullName,
                                                       TrakingId = t1.TrakingId,
                                                       CustomerName = t3.Name==null?"": t3.Name,
                                                       SalesOrderNo = t1.SalesOrderNo,
                                                       SalesDate = t1.SalesDate,
                                                       SupplierPaymentMethodEnumFK = t1.SupplierPaymentMethodEnumFK,
                                                       DeliveryDate = t1.DeliveryDate,
                                                       DeliveryAddress = t1.DeliveryAddress,
                                                       IsSubmited = t1.IsSubmited,
                                                       IsCancel = t1.IsCancel,
                                                   }).OrderBy(f=>f.IsSubmited).AsQueryable());
            if (user.UserType == 2)
            {
                model.datalist = model.datalist.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.datalist = model.datalist.Where(t =>
                    t.CustomerName.ToLower().Contains(sarchString) ||
                    t.SalesOrderNo.ToLower().Contains(sarchString)
                );
            }
            int resCount = model.datalist.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.datalist.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<SalesViewModel> pagedModel = new PagedModel<SalesViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<SalesViewModel> GetSaleOrder(long id)
        {
            SalesViewModel model = new SalesViewModel();

            var user = await workContext.GetCurrentUserAsync();
            model = await Task.Run(() => (from t1 in dbContext.SalesOrder
                                          join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                          join vendor in dbContext.Vendor on t1.CustomerId equals vendor.Id
                                          where t1.IsActive == true && t1.Id == id
                                          select new SalesViewModel
                                          {
                                              Id = t1.Id,
                                              UserName = t2.FullName,
                                              TrakingId = t1.TrakingId,
                                              CustomerName = vendor.Name,
                                              CustomerId = vendor.Id,
                                              CustomerMobile = vendor.Mobile,
                                              CustomerEmail = vendor.Email,
                                              CustomerAddress = vendor.Address,
                                              SalesOrderNo = t1.SalesOrderNo,
                                              SalesDate = t1.SalesDate,
                                              SupplierPaymentMethodEnumFK = t1.SupplierPaymentMethodEnumFK,
                                              DeliveryDate = t1.DeliveryDate,
                                              DeliveryAddress = t1.DeliveryAddress,
                                              IsSubmited = t1.IsSubmited,
                                              Description = t1.Description,
                                              TermsAndCondition = t1.TermsAndCondition,
                                              IsCancel = t1.IsCancel,
                                              OtherCharge = t1.OtherCharge,
                                              UserAddress = user.Address,
                                              UserMobile = user.PhoneNumber,
                                              UserEmail = user.Email,
                                              ImageUrl = user.ImageUrl,
                                              DiscountAmount=t1.DiscountAmount,
                                              Discountpercentages = t1.Discountpercentages,
                                          }).FirstOrDefaultAsync());

            model.MappVm = await Task.Run(() => (from t1 in dbContext.SalesOrderDetails
                                                 join t2 in dbContext.Product on t1.ProductId equals t2.Id
                                                 join t3 in dbContext.ProductSubCategory on t2.ProductSubCategoryId equals t3.Id
                                                 join t4 in dbContext.ProductCategory on t2.ProductCategoryId equals t4.Id
                                                 where t1.IsActive == true && t1.SalesOrderId == model.Id
                                                 select new SalesOrderDetailsViewModel
                                                 {
                                                     Id = t1.Id,
                                                     ProductId = t1.ProductId,
                                                     SalesAmount = t1.SalesAmount,
                                                     SalesOrderId = t1.SalesOrderId,
                                                     SalesQty = t1.SalesQty,
                                                     SalesRate = t1.SalesRate,
                                                     PackSize = t1.PackSize,
                                                     ProductName = t4.Name + "-" + t3.Name + "-" + t2.ProductName,
                                                     UnitName = t2.UnitName,
                                                     Discount_Persentage=t1.Discount_Persentage,
                                                     Discount_Amount=t1.Discount_Amount

                                                 }).ToListAsync());
            return model;
        }

        public async Task<long> UpdateSaleOrder(SalesViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            SalesOrder order = await dbContext.SalesOrder.FirstOrDefaultAsync(d => d.Id == model.Id);

            if (model.CustomerId == 0)
            {
                Vendor vendor = new Vendor();
                vendor.Name = model.CustomerName;
                vendor.Mobile = model.CustomerMobile;
                vendor.Email = model.CustomerEmail;
                vendor.Address = model.CustomerAddress;
                vendor.VendorType = 2;
                vendor.CreatedBy = user.FullName;
                vendor.TrakingId = user.Id;
                vendor.CreatedOn = BaTime;
                dbContext.Add(vendor);
                dbContext.SaveChanges();
                model.CustomerId = vendor.Id;
            }
            order.CustomerId = model.CustomerId;
            order.SalesDate = model.SalesDate;
            order.DeliveryDate = model.DeliveryDate;
            order.DeliveryAddress = model.DeliveryAddress;
            order.OtherCharge = model.OtherCharge;
            order.Description = model.Description;
            order.TermsAndCondition = model.TermsAndCondition;
            order.UpdatedBy = user.FullName;
            order.UpdatedOn = BaTime;
            dbContext.Entry(order).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return order.Id;
        }

    }
}
