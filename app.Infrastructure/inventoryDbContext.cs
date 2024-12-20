using app.EntityModel.CoreModel;
using app.EntityModel.DatabaseView;
using app.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace app.Infrastructure
{
    public class inventoryDbContext : IdentityDbContext<ApplicationUser>
    {
        public inventoryDbContext(DbContextOptions<inventoryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            FixedData.SeedData(builder);
            base.OnModelCreating(builder);
        }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<MainMenu> MainMenu { get; set; }
        public virtual DbSet<Userpermissions> Userpermissions { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductSubCategory> ProductSubCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Upazila> Upazila { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<VoucherType> VoucherType { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<VoucherDetails> VoucherDetails { get; set; }
        public virtual DbSet<StockInfo> StockInfo { get; set; }
        public virtual DbSet<UserProduct> UserProduct { get; set; }
        public virtual DbSet<AccountHeads> AccountHeads { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }
        public virtual DbSet<SalesOrder> SalesOrder { get; set; }
        public virtual DbSet<SalesOrderDetails> SalesOrderDetails { get; set; }
        public virtual DbSet<SalesReturn> SalesReturn { get; set; }
        public virtual DbSet<SalesReturnDetails> SalesReturnDetails { get; set; }
        public virtual DbSet<PurchaseReturn> PurchaseReturn { get; set; }
        public virtual DbSet<PurchaseReturnDetails> PurchaseReturnDetails { get; set; }
        public virtual DbSet<BillGenerated> BillGenerated { get; set; }
        public virtual DbSet<CostCenters> CostCenters { get; set; }
        public virtual DbSet<VoucherTypes> VoucherTypes { get; set; }
        public virtual DbSet<Vouchers> Vouchers { get; set; }
        public virtual DbSet<VouchersLines> VouchersLines { get; set; }
        public virtual DbSet<CoATypes> CoATypes { get; set; }
        public virtual DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        [NotMapped]
        public virtual DbSet<StockView> StockView { get; set; }
        [NotMapped]
        public virtual DbSet<SalesViewReport> SalesViewReport { get; set; }
        [NotMapped]
        public virtual DbSet<PurchesViewRepot> PurchesViewRepot { get; set; }
        [NotMapped]
        public virtual DbSet<ProfitandLossStatement> ProfitandLossStatement { get; set; }



    }
}
