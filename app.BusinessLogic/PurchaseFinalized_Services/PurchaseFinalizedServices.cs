using app.Infrastructure.Auth;
using app.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.Infrastructure.Repository;
using app.EntityModel.CoreModel;
using Microsoft.EntityFrameworkCore;
using app.Utility;
using System.Runtime.Intrinsics.X86;

namespace app.Services.PurchaseFinalized_Services
{
    public class PurchaseFinalizedServices : IPurchaseFinalizedServices
    {
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public PurchaseFinalizedServices(inventoryDbContext dbContext, IWorkContext workContext)
        {
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<long> GetPurchaseFinalizedAsync(long id)
        {
            if (id == 0) { return -1; }
            var user = await workContext.GetCurrentUserAsync();
            PurchaseOrder purchase = await dbContext.PurchaseOrder.FindAsync(id);
            VoucherType voucherType = dbContext.VoucherType.Where(x => x.Code == "PV").FirstOrDefault();
            List<PurchaseOrderDetails> details = await dbContext.PurchaseOrderDetails.Where(d => d.PurchaseOrderId == purchase.Id && d.IsActive == true).ToListAsync();
            decimal totalcost = purchase.TransportCharges + purchase.BankCharg + purchase.OtherCharge;
            decimal total = details.Sum(item => item.PurchaseAmount);
            if (details.Count == 0) { return -2; }
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Voucher voucher = new Voucher();
                    voucher.VoucherDate = purchase.PurchaseDate;
                    voucher.VoucherNo = GetVoucherNo(voucherType.Id,voucherType.Code, user.Id, purchase.PurchaseDate);
                    voucher.VoucherTypeId=voucherType.Id;
                    voucher.VendorId = purchase.SupplierId;
                    voucher.ReferenceId = purchase.Id;
                    voucher.Narration = "OrderNo:" + purchase.PurchaseOrderNo+"-" +purchase.Description;
                    voucher.CreatedBy = user.FullName;    
                    voucher.TrakingId = user.Id;    
                    voucher.CreatedOn=DateTime.Now;
                    voucher.IsActive = true;
                    voucher.IsSubmitted = true; 
                    dbContext.Voucher.Add(voucher);
                    dbContext.SaveChanges();  
                    List<VoucherDetails> list = new List<VoucherDetails>();
                    foreach (var item in details)
                    {
                        VoucherDetails voucherDetails = new VoucherDetails();
                        voucherDetails.VoucherId = voucher.Id;
                        voucherDetails.ProductId = item.ProductId;
                        voucherDetails.ReferenceId = voucher.ReferenceId;
                        voucherDetails.DebitAmount=item.PurchaseAmount;
                        voucherDetails.CreditAmount=0;
                        voucherDetails.CreatedBy = user.FullName;
                        voucherDetails.TrakingId = user.Id;
                        voucherDetails.CreatedOn = DateTime.Now;
                        voucherDetails.IsActive = true;
                        list.Add(voucherDetails);
                    }
                    dbContext.VoucherDetails.AddRange(list);
                    dbContext.SaveChanges();
                    List<StockInfo> stockInfos = new List<StockInfo>();
                    List<UserProduct> uproduct = new List<UserProduct>();
                    foreach (var item in details) {
                        decimal costingprice = ((((totalcost * item.PurchaseAmount) / total) + item.PurchaseAmount) /item.PurchaseQty);
                        StockInfo stock=new StockInfo();
                        UserProduct userProduct =dbContext.UserProduct.FirstOrDefault(f=>f.ProductId==item.ProductId&&f.TrakingId== user.Id);

                        stock.StockTypeId = (int)StockType.PV;
                        stock.ProductId = item.ProductId;
                        stock.ReferenceId = purchase.Id;
                        stock.ReferenceNo = purchase.PurchaseOrderNo;
                        stock.InQty = item.PurchaseQty;
                        stock.InPrice = item.PurchaseRate;
                        stock.OutQty = 0;
                        stock.OutPrice = 0;
                        stock.CreatedBy = user.FullName;
                        stock.TrakingId = user.Id;
                        stock.CreatedOn = DateTime.Now;
                        stock.IsActive = true;
                        stock.ReceivedDate = purchase.PurchaseDate;
                        stock.CogsPrice = costingprice;
                        dbContext.StockInfo.Add(stock);
                        dbContext.SaveChanges();
                        var AVGPrice = magAVGPrice(user.Id,item.ProductId);
                        userProduct.AVGPrice = (decimal)AVGPrice;
                        dbContext.Entry(userProduct).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    purchase.IsSubmited = true; ;
                    purchase.UpdatedBy = user.FullName;
                    purchase.UpdatedOn = DateTime.Now;
                    dbContext.Entry(purchase).State = EntityState.Modified;
                    dbContext.SaveChanges();                  
                    scope.Commit();
                    return voucher.Id;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return 0;
                }
            }
        }

        private object magAVGPrice(string id,long product)
        {
            var stockInfos = dbContext.StockInfo.Where(d=>d.TrakingId==id&&d.IsActive==true&&d.ProductId==product).ToList();
            decimal inqty=stockInfos.Select(d=>d.InQty).Sum();   
            decimal Outqty=stockInfos.Select(d=>d.OutQty).Sum();

            decimal InValue = stockInfos.Select(d => d.InQty*d.CogsPrice).Sum();
            decimal OutValue = stockInfos.Select(d => d.OutQty*d.CogsPrice).Sum();

            decimal balanceQty = (inqty - Outqty);
            decimal totalValue = InValue - OutValue;
            decimal Abgprice = totalValue / balanceQty;
            return Abgprice;
        }

        public string GetVoucherNo(int voucherTypeId,string Code,string userid, DateTime voucherDate)
        {
            string voucherNo = string.Empty;
            int vouchersCount = dbContext.Voucher.Where(x => x.VoucherTypeId == voucherTypeId && x.VoucherDate.Month == voucherDate.Month
            && x.VoucherDate.Year == voucherDate.Year&&x.TrakingId==userid).Count();
            vouchersCount++;
            voucherNo =Code + "-" + vouchersCount.ToString().PadLeft(4, '0');
            return voucherNo;
        }

        public async Task<long> GetSalesFinalizedAsync(long id)
        {
            if (id == 0) { return -1; }
            var user = await workContext.GetCurrentUserAsync();
            SalesOrder salse = await dbContext.SalesOrder.FindAsync(id);
            VoucherType voucherType = dbContext.VoucherType.Where(x => x.Code == "SV").FirstOrDefault();
            List<SalesOrderDetails> details = await dbContext.SalesOrderDetails.Where(d => d.SalesOrderId == salse.Id&& d.IsActive==true).ToListAsync();          
            decimal total = details.Sum(item => item.SalesAmount);
            if (details.Count == 0) { return -2; }
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Voucher voucher = new Voucher();
                    voucher.VoucherDate = salse.SalesDate;
                    voucher.VoucherNo = GetVoucherNo(voucherType.Id, voucherType.Code, user.Id, salse.SalesDate);
                    voucher.VoucherTypeId = voucherType.Id;
                    voucher.VendorId = salse.CustomerId;
                    voucher.ReferenceId = salse.Id;
                    voucher.Narration = "OrderNo:" + salse.SalesOrderNo + "-" + salse.Description;
                    voucher.CreatedBy = user.FullName;
                    voucher.TrakingId = user.Id;
                    voucher.CreatedOn = DateTime.Now;
                    voucher.IsActive = true;
                    voucher.IsSubmitted = true;
                    dbContext.Voucher.Add(voucher);
                    dbContext.SaveChanges();
                    List<VoucherDetails> list = new List<VoucherDetails>();
                    foreach (var item in details)
                    {
                        
                        VoucherDetails voucherDetails = new VoucherDetails();
                        voucherDetails.VoucherId = voucher.Id;
                        voucherDetails.ProductId = item.ProductId;
                        voucherDetails.ReferenceId = voucher.ReferenceId;
                        voucherDetails.DebitAmount = item.SalesAmount;
                        voucherDetails.CreditAmount = 0;
                        voucherDetails.CreatedBy = user.FullName;
                        voucherDetails.TrakingId = user.Id;
                        voucherDetails.CreatedOn = DateTime.Now;
                        voucherDetails.IsActive = true;
                        list.Add(voucherDetails);
                    }
                    dbContext.VoucherDetails.AddRange(list);
                    dbContext.SaveChanges();
                    List<StockInfo> stockInfos = new List<StockInfo>();
                    List<UserProduct> uproduct = new List<UserProduct>();
                    foreach (var item in details)
                    {
                        var product = dbContext.UserProduct.FirstOrDefault(f => f.ProductId == item.ProductId && f.TrakingId == user.Id);
                        StockInfo stock = new StockInfo();
                        stock.StockTypeId = (int)StockType.SV;
                        stock.ProductId = item.ProductId;
                        stock.ReferenceId = salse.Id;
                        stock.ReferenceNo = salse.SalesOrderNo;
                        stock.InQty = 0;
                        stock.InPrice = 0;
                        stock.OutQty = item.SalesQty;
                        stock.OutPrice = product.AVGPrice;
                        stock.CreatedBy = user.FullName;
                        stock.TrakingId = user.Id;
                        stock.CreatedOn = DateTime.Now;
                        stock.IsActive = true;
                        stock.ReceivedDate = salse.SalesDate;
                        dbContext.StockInfo.Add(stock);
                        dbContext.SaveChanges();
                    }
                    salse.IsSubmited = true; ;
                    salse.UpdatedBy = user.FullName;
                    salse.UpdatedOn = DateTime.Now;
                    dbContext.Entry(salse).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    scope.Commit();
                    return voucher.Id;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return 0;
                }
            }
        }

        public async Task<long> GetSalesReturnFinalizedAsync(long id)
        {

            if (id == 0) { return -1; }
            var user = await workContext.GetCurrentUserAsync();
            SalesReturn salse = await dbContext.SalesReturn.FindAsync(id);
            VoucherType voucherType = dbContext.VoucherType.Where(x => x.Code == "SRV").FirstOrDefault();
            List<SalesReturnDetails> details = await dbContext.SalesReturnDetails.Where(d => d.SalesReturnId == salse.Id && d.IsActive == true).ToListAsync();
            decimal total = details.Sum(item => item.ReturnAmount);
            if (details.Count == 0) { return -2; }
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Voucher voucher = new Voucher();
                    voucher.VoucherDate = salse.SalesReturnDate;
                    voucher.VoucherNo = GetVoucherNo(voucherType.Id, voucherType.Code, user.Id, salse.SalesReturnDate);
                    voucher.VoucherTypeId = voucherType.Id;
                    voucher.VendorId = salse.CustomerId;
                    voucher.ReferenceId = salse.Id;
                    voucher.Narration = "OrderNo:" + salse.SalesReturnNo + "-" + salse.Reason;
                    voucher.CreatedBy = user.FullName;
                    voucher.TrakingId = user.Id;
                    voucher.CreatedOn = DateTime.Now;
                    voucher.IsActive = true;
                    voucher.IsSubmitted = true;
                    dbContext.Voucher.Add(voucher);
                    dbContext.SaveChanges();
                    List<VoucherDetails> list = new List<VoucherDetails>();
                    foreach (var item in details)
                    {
                        VoucherDetails voucherDetails = new VoucherDetails();
                        voucherDetails.VoucherId = voucher.Id;
                        voucherDetails.ProductId = item.ProductId;
                        voucherDetails.ReferenceId = voucher.ReferenceId;
                        voucherDetails.DebitAmount = item.ReturnAmount;
                        voucherDetails.CreditAmount = 0;
                        voucherDetails.CreatedBy = user.FullName;
                        voucherDetails.TrakingId = user.Id;
                        voucherDetails.CreatedOn = DateTime.Now;
                        voucherDetails.IsActive = true;
                        list.Add(voucherDetails);
                    }
                    dbContext.VoucherDetails.AddRange(list);
                    dbContext.SaveChanges();
                    List<StockInfo> stockInfos = new List<StockInfo>();
                    List<UserProduct> uproduct = new List<UserProduct>();
                    foreach (var item in details)
                    {

                        StockInfo stock = new StockInfo();
                        stock.StockTypeId = (int)StockType.SRV;
                        stock.ProductId = item.ProductId;
                        stock.ReferenceId = salse.Id;
                        stock.ReferenceNo = salse.SalesReturnNo;
                        stock.InQty = 0;
                        stock.InPrice = 0;
                        stock.OutQty = 0;
                        stock.OutPrice = 0;
                        stock.PurchesReturnQty = 0;
                        stock.PurchesSaleReturnPrice = 0;
                        stock.SaleReturnQty = item.ReturnQty;
                        stock.SaleReturnPrice = item.ReturnRate;
                        stock.CreatedBy = user.FullName;
                        stock.TrakingId = user.Id;
                        stock.CreatedOn = DateTime.Now;
                        stock.IsActive = true;
                        stock.ReceivedDate = salse.SalesReturnDate;
                        dbContext.StockInfo.Add(stock);
                        dbContext.SaveChanges();
                    }
                    salse.IsSubmited = true; ;
                    salse.UpdatedBy = user.FullName;
                    salse.UpdatedOn = DateTime.Now;
                    dbContext.Entry(salse).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    scope.Commit();
                    return voucher.Id;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return 0;
                }
            }
        }

        public async Task<long> GetPurchaseReturnFinalizedAsync(long id)
        {
            if (id == 0) { return -1; }
            var user = await workContext.GetCurrentUserAsync();
            PurchaseReturn purchaes = await dbContext.PurchaseReturn.FindAsync(id);
            VoucherType voucherType = dbContext.VoucherType.Where(x => x.Code == "PRV").FirstOrDefault();
            List<PurchaseReturnDetails> details = await dbContext.PurchaseReturnDetails.Where(d => d.PurchaseReturnId == purchaes.Id && d.IsActive == true).ToListAsync();
            decimal total = details.Sum(item => item.PurchaseReturnAmount);
            if (details.Count == 0) { return -2; }
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Voucher voucher = new Voucher();
                    voucher.VoucherDate = purchaes.PurchaseReturnDate;
                    voucher.VoucherNo = GetVoucherNo(voucherType.Id, voucherType.Code, user.Id, purchaes.PurchaseReturnDate);
                    voucher.VoucherTypeId = voucherType.Id;
                    voucher.VendorId = purchaes.SupplierId;
                    voucher.ReferenceId = purchaes.Id;
                    voucher.Narration = "Return No:" + purchaes.PurchaseReturnNo + "-" + purchaes.Reason;
                    voucher.CreatedBy = user.FullName;
                    voucher.TrakingId = user.Id;
                    voucher.CreatedOn = DateTime.Now;
                    voucher.IsActive = true;
                    voucher.IsSubmitted = true;
                    dbContext.Voucher.Add(voucher);
                    dbContext.SaveChanges();
                    List<VoucherDetails> list = new List<VoucherDetails>();
                    foreach (var item in details)
                    {
                        VoucherDetails voucherDetails = new VoucherDetails();
                        voucherDetails.VoucherId = voucher.Id;
                        voucherDetails.ProductId = item.ProductId;
                        voucherDetails.ReferenceId = voucher.ReferenceId;
                        voucherDetails.DebitAmount = 0;
                        voucherDetails.CreditAmount = item.PurchaseReturnAmount;
                        voucherDetails.CreatedBy = user.FullName;
                        voucherDetails.TrakingId = user.Id;
                        voucherDetails.CreatedOn = DateTime.Now;
                        voucherDetails.IsActive = true;
                        list.Add(voucherDetails);
                    }
                    dbContext.VoucherDetails.AddRange(list);
                    dbContext.SaveChanges();
                    List<StockInfo> stockInfos = new List<StockInfo>();
                    List<UserProduct> uproduct = new List<UserProduct>();
                    foreach (var item in details)
                    {

                        StockInfo stock = new StockInfo();
                        stock.StockTypeId = (int)StockType.SRV;
                        stock.ProductId = item.ProductId;
                        stock.ReferenceId = purchaes.Id;
                        stock.ReferenceNo = purchaes.PurchaseReturnNo;
                        stock.InQty = 0;
                        stock.InPrice = 0;
                        stock.OutQty = 0;
                        stock.OutPrice = 0;
                        stock.PurchesReturnQty = item.ReturnQty;
                        stock.PurchesSaleReturnPrice = item.PurchaseReturnRate;
                        stock.SaleReturnQty = 0;
                        stock.SaleReturnPrice = 0;
                        stock.CreatedBy = user.FullName;
                        stock.TrakingId = user.Id;
                        stock.CreatedOn = DateTime.Now;
                        stock.IsActive = true;
                        stock.ReceivedDate = purchaes.PurchaseReturnDate;
                        dbContext.StockInfo.Add(stock);
                        dbContext.SaveChanges();
                    }
                    purchaes.IsSubmited = true; ;
                    purchaes.UpdatedBy = user.FullName;
                    purchaes.UpdatedOn = DateTime.Now;
                    dbContext.Entry(purchaes).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    scope.Commit();
                    return voucher.Id;
                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return 0;
                }
            }


        }
    }
}
