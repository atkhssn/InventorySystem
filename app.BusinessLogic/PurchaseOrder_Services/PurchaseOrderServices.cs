using app.EntityModel.CoreModel;
using app.Infrastructure.Auth;
using app.Infrastructure.Repository;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.Utility.Miscellaneous;
using app.Services.ProductSubCategory_Service;

namespace app.Services.PurchaseOrder_Services
{
    public class PurchaseOrderServices : IPurchaseOrderServices
    {

        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public PurchaseOrderServices(inventoryDbContext dbContext,
            IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<long> AddPurchaseOrder(PurchaseOrderViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var poMax = await dbContext.PurchaseOrder.Where(x => x.TrakingId == user.Id).CountAsync() + 1;
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var poCid = @"PUR-" +
                    DateTime.Now.ToString("yy") + "-" +
                    DateTime.Now.ToString("MM") + "-" +
                    DateTime.Now.ToString("dd") + "-" +
                    poMax.ToString().PadLeft(2, '0');
                    PurchaseOrder order = new PurchaseOrder();
                    order.PurchaseOrderNo = poCid;
                    order.SupplierId = model.SupplierId;
                    order.PurchaseDate = model.PurchaseDate;
                    order.DeliveryDate = model.DeliveryDate;
                    order.DeliveryAddress = model.DeliveryAddress;
                    order.BankCharg = model.BankCharg;
                    order.TransportCharges = model.TransportCharges;
                    order.OtherCharge = model.OtherCharge;
                    order.SupplierPaymentMethodEnumFK = model.SupplierPaymentMethodEnumFK;
                    order.Description = model.Description;
                    order.TermsAndCondition = model.TermsAndCondition;
                    order.Status = model.Status;
                    order.CreatedBy = user.FullName;
                    order.TrakingId = user.Id;
                    order.CreatedOn = BaTime;
                    dbContext.PurchaseOrder.Add(order);
                    dbContext.SaveChanges();
                    List<PurchaseOrderDetails> purchaseOrderDetails = new List<PurchaseOrderDetails>();
                    foreach (var item in model.MappVm)
                    {
                        PurchaseOrderDetails details = new PurchaseOrderDetails();
                        details.PurchaseOrderId = order.Id;
                        details.ProductId = item.ProductId;
                        details.PurchaseQty = item.PurchaseQty;
                        details.PurchaseRate = item.PurchaseRate;
                        details.PurchaseAmount = item.PurchaseAmount;
                        details.PackSize = item.PackSize;
                        details.CreatedBy = user.FullName;
                        details.CreatedOn = BaTime;
                        details.TrakingId = user.Id;
                        purchaseOrderDetails.Add(details);
                    }
                    dbContext.PurchaseOrderDetails.AddRange(purchaseOrderDetails);
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

        public async Task<long> AddPurchaseOrderDetalies(PurchaseOrderDetailsViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            PurchaseOrderDetails details = new PurchaseOrderDetails();
            details.PurchaseOrderId = model.PurchaseOrderId;
            details.ProductId = model.ProductId;
            details.PurchaseQty = model.PurchaseQty;
            details.PurchaseRate = model.PurchaseRate;
            details.PurchaseAmount = model.PurchaseAmount;
            details.PackSize = model.PackSize;
            details.CreatedBy = user.FullName;
            details.CreatedOn = BaTime;
            details.TrakingId = user.Id;
            dbContext.PurchaseOrderDetails.Add(details);
            await dbContext.SaveChangesAsync();
            return model.PurchaseOrderId;
        }

        public async Task<long> DeletePurchaseOrderDetalies(long id)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            var result= await dbContext.PurchaseOrderDetails.FirstOrDefaultAsync(x => x.Id == id);  
            result.IsActive = false;
            result.UpdatedOn= BaTime;
            result.CreatedBy = user.FullName;   
            dbContext.Entry(result).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return result.PurchaseOrderId;
        }

        public async Task<PagedModel<PurchaseOrderViewModel>> GetPagedListAsync(int page, int pageSize, string sarchString)
        {
            PurchaseOrderViewModel model = new PurchaseOrderViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.datalist = await Task.Run(() => (from t1 in dbContext.PurchaseOrder
                                                   join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                   join t3 in dbContext.Vendor on t1.SupplierId equals t3.Id
                                                   where t1.IsActive == true && t1.TrakingId == user.Id 
                                                   select new PurchaseOrderViewModel
                                                   {
                                                       Id = t1.Id,
                                                       UserName = t2.FullName,
                                                       TrakingId = t1.TrakingId,
                                                       SupplierName = t3.Name,
                                                       PurchaseOrderNo = t1.PurchaseOrderNo,
                                                       PurchaseDate = t1.PurchaseDate,
                                                       SupplierPaymentMethodEnumFK = t1.SupplierPaymentMethodEnumFK,
                                                       DeliveryDate = t1.DeliveryDate,
                                                       DeliveryAddress = t1.DeliveryAddress,
                                                       IsSubmited = t1.IsSubmited,
                                                       IsCancel = t1.IsCancel,
                                                       IsHold = t1.IsHold,
                                                   }).OrderBy(d=>d.IsSubmited).AsQueryable());
            if (user.UserType == 2)
            {
                model.datalist = model.datalist.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.datalist = model.datalist.Where(t =>
                    t.SupplierName.ToLower().Contains(sarchString) ||
                    t.PurchaseOrderNo.ToLower().Contains(sarchString) 
                );
            }
            int resCount = model.datalist.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.datalist.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<PurchaseOrderViewModel> pagedModel = new PagedModel<PurchaseOrderViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<PurchaseOrderViewModel> GetPurchaseOrder(long id)
        {
            PurchaseOrderViewModel model = new PurchaseOrderViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model = await Task.Run(() => (from t1 in dbContext.PurchaseOrder
                                          join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                          join vendor in dbContext.Vendor on t1.SupplierId equals vendor.Id
                                          where t1.IsActive == true&&t1.Id == id    
                                          select new PurchaseOrderViewModel
                                          {
                                              Id = t1.Id,
                                              UserName = t2.FullName,
                                              UserEmail = t2.Email,
                                              UserMobile = t2.PhoneNumber,
                                              UserAddress = t2.Address,
                                              ImageUrl = t2.ImageUrl,
                                              TrakingId = t1.TrakingId,
                                              SupplierName = vendor.Name,
                                              SupplierId = t1.SupplierId,
                                              SupplierMobile = vendor.Mobile,
                                              SupplierAddress = vendor.Address,
                                              PurchaseOrderNo = t1.PurchaseOrderNo,
                                              PurchaseDate = t1.PurchaseDate,
                                              SupplierPaymentMethodEnumFK = t1.SupplierPaymentMethodEnumFK,
                                              DeliveryDate = t1.DeliveryDate,
                                              DeliveryAddress = t1.DeliveryAddress,
                                              Description=t1.Description,
                                              BankCharg=t1.BankCharg,
                                              TransportCharges = t1.TransportCharges,   
                                              OtherCharge=t1.OtherCharge,
                                              TermsAndCondition=t1.TermsAndCondition,
                                              IsSubmited = t1.IsSubmited,
                                              IsCancel = t1.IsCancel,
                                              IsHold = t1.IsHold,
                                          }).FirstOrDefaultAsync());

            model.MappVm = await Task.Run(() => (from t1 in dbContext.PurchaseOrderDetails
                                          join t2 in dbContext.Product on t1.ProductId equals t2.Id
                                          join t3 in dbContext.ProductSubCategory on t2.ProductSubCategoryId equals t3.Id
                                          join t4 in dbContext.ProductCategory on t2.ProductCategoryId equals t4.Id
                                          where t1.IsActive == true&&t1.PurchaseOrderId==model.Id
                                          select new PurchaseOrderDetailsViewModel
                                          {
                                              Id = t1.Id,
                                              ProductId=t1.ProductId,
                                              PurchaseAmount=t1.PurchaseAmount,
                                              PurchaseQty=t1.PurchaseQty,   
                                              PurchaseRate=t1.PurchaseRate, 
                                              PackSize=t1.PackSize,
                                              PurchaseOrderId=t1.PurchaseOrderId,   
                                              ProductName=t4.Name+"-"+t3.Name+"-"+t2.ProductName,
                                              UnitName=t2.UnitName                                  
                                          }).AsQueryable());
            return model;   
        }

        public async Task<long> UpdatePurchaseOrder(PurchaseOrderViewModel model)
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            var user = await workContext.GetCurrentUserAsync();
            PurchaseOrder order = await dbContext.PurchaseOrder.FirstOrDefaultAsync(d=>d.Id==model.Id);
            order.SupplierId = model.SupplierId;
            order.PurchaseDate = model.PurchaseDate;
            order.DeliveryDate = model.DeliveryDate;
            order.DeliveryAddress = model.DeliveryAddress;
            order.BankCharg = model.BankCharg;
            order.TransportCharges = model.TransportCharges;
            order.OtherCharge = model.OtherCharge;
            order.SupplierPaymentMethodEnumFK = model.SupplierPaymentMethodEnumFK;
            order.Description = model.Description;
            order.TermsAndCondition = model.TermsAndCondition;
            order.UpdatedBy = user.FullName;
            order.UpdatedOn = BaTime;
            dbContext.Entry(order).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return  order.Id;   
        }
    }
}
