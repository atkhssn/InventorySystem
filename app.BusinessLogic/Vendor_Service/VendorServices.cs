using app.EntityModel.CoreModel;
using app.Infrastructure.Auth;
using app.Infrastructure.Repository;
using app.Infrastructure;
using app.Utility.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.Services.ProductCategory_Services;
using app.Utility;

namespace app.Services.Vendor_Service
{
    public class VendorServices : IVendorService
    {
        private readonly IEntityRepository<Vendor> _entityRepository;
        private readonly inventoryDbContext dbContext;
        private readonly IWorkContext workContext;
        public VendorServices(IEntityRepository<Vendor> entityRepository, inventoryDbContext dbContext, IWorkContext workContext)
        {
            _entityRepository = entityRepository;
            this.dbContext = dbContext;
            this.workContext = workContext;
        }
        public async Task<int> AddRecord(VendorViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            var checkphone = await dbContext.Vendor.FirstOrDefaultAsync(f => f.Mobile == model.Mobile && f.TrakingId == user.Id && f.VendorType == model.VendorType);
            if (checkphone == null)
            {
                Vendor vendor = new Vendor();
                vendor.Name = model.Name;
                vendor.Mobile = model.Mobile;
                vendor.Email = model.Email;
                vendor.Address = model.Address;
                vendor.VendorType = model.VendorType;
                vendor.NID = model.NID;
                vendor.ContactName = model.ContactName;
                vendor.CreditLimit = model.CreditLimit;
                vendor.ZoneId = model.ZoneId;
                vendor.SubZoneId = model.SubZoneId;
                vendor.DivisionId = model.DivisionId;
                vendor.DistrictId = model.DistrictId;
                vendor.UpazilaId = model.UpazilaId;
                vendor.CustomerType = model.CustomerType;
                vendor.PaymentType = model.PaymentType;
                vendor.IsForeign = model.IsForeign;
                vendor.SecurityAmount = model.SecurityAmount;
                vendor.ACName = model.ACName;
                vendor.ACNo = model.ACNo;
                vendor.BankName = model.BankName;
                vendor.BranchName = model.BranchName;
                await _entityRepository.AddAsync(vendor);
                return 2;
            }
            return 1;
        }

        public async Task<bool> DeleteRecord(long id)
        {
            var result = await _entityRepository.GetByIdAsync(id);
            result.IsActive = false;
            await _entityRepository.UpdateAsync(result);
            return true;
        }

        public async Task<VendorViewModel> GetAllCustomerRecord(int id)
        {
            VendorViewModel model = new VendorViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.vendorList = await Task.Run(() => (from t1 in dbContext.Vendor
                                                     join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                     where t1.IsActive == true && t1.VendorType == 2 && t1.TrakingId == user.Id 
                                                     select new VendorViewModel
                                                     {
                                                         Id = t1.Id,
                                                         Name = t1.Name,
                                                         Mobile = t1.Mobile,
                                                         Email = t1.Email,
                                                         NID = t1.NID,
                                                         ContactName = t1.ContactName,
                                                         Address = t1.Address,
                                                         UserName = t2.FullName,
                                                         TrakingId = t1.TrakingId,
                                                     }).AsQueryable());
            return model;
        }

        public async Task<VendorViewModel> GetAllSupplierRecord(int id)
        {
            VendorViewModel model = new VendorViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.vendorList = await Task.Run(() => (from t1 in dbContext.Vendor
                                                     join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                     where t1.IsActive == true && t1.VendorType == 1 && t1.TrakingId==user.Id
                                                     select new VendorViewModel
                                                     {
                                                         Id = t1.Id,
                                                         Name = t1.Name,
                                                         Mobile = t1.Mobile,
                                                         Email = t1.Email,
                                                         NID = t1.NID,
                                                         ContactName = t1.ContactName,
                                                         Address = t1.Address,
                                                         UserName = t2.FullName,
                                                         TrakingId = t1.TrakingId,
                                                     }).AsQueryable());
            return model;
        }

        public async Task<PagedModel<VendorViewModel>> GetPagedListAsync(int page, int pageSize, int vendorType,string sarchString)
        {
            VendorViewModel model = new VendorViewModel();
            var user = await workContext.GetCurrentUserAsync();
            model.vendorList = await Task.Run(() => (from t1 in dbContext.Vendor
                                                     join t2 in dbContext.Users on t1.TrakingId equals t2.Id
                                                     where t1.IsActive == true&&t1.VendorType == vendorType && t1.TrakingId == user.Id
                                                     select new VendorViewModel
                                                     {
                                                         Id = t1.Id,
                                                         Name = t1.Name == null ? "" : t1.Name,
                                                         Mobile = t1.Mobile,    
                                                         Email =t1.Email == null ? "" : t1.Email,
                                                         NID = t1.NID==null?"":t1.NID,
                                                         ContactName = t1.ContactName==null?"": t1.ContactName,  
                                                         Address = t1.Address,  
                                                         UserName = t2.FullName,
                                                         TrakingId = t1.TrakingId,
                                                     }).AsQueryable());
            if (user.UserType == 2)
            {
                model.vendorList = model.vendorList.Where(f => f.TrakingId == user.Id).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(sarchString))
            {
                sarchString = sarchString.Trim().ToLower();
                model.vendorList = model.vendorList.Where(t =>
                    t.Name.ToString().ToLower().Contains(sarchString) ||
                    t.UserName.ToString().ToLower().Contains(sarchString) ||
                    t.Mobile.ToString().ToLower().Contains(sarchString) ||
                    t.NID.ToString().ToLower().Contains(sarchString) ||
                    t.Email.ToString().ToLower().Contains(sarchString) ||
                    t.ContactName.ToString().ToLower().Contains(sarchString)||
                    t.Address.ToString().ToLower().Contains(sarchString)
                ).AsQueryable();
            }
            int resCount = model.vendorList.Count();
            var pagers = new PagedList(resCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            var pagedList = model.vendorList.Skip(recSkip).Take(pagers.PageSize).ToList();
            int FirstSerialNumber = ((page * pageSize) - pageSize) + 1;
            PagedModel<VendorViewModel> pagedModel = new PagedModel<VendorViewModel>()
            {
                Models = pagedList,
                FirstSerialNumber = FirstSerialNumber,
                PagedList = pagers,
                TotalEntity = resCount,
                UserType = user.UserType,
            };
            return pagedModel;
        }

        public async Task<VendorViewModel> GetRecord(long id)
        {
            VendorViewModel vendor = new VendorViewModel();
            Vendor model = await _entityRepository.GetByIdAsync(id);
            vendor.Name = model.Name;
            vendor.Mobile = model.Mobile;
            vendor.Email = model.Email;
            vendor.Address = model.Address;
            vendor.VendorType = model.VendorType;
            vendor.PaymentType = model.PaymentType;
            vendor.NID = model.NID;
            vendor.IsForeign = model.IsForeign;
            vendor.ContactName = model.ContactName;
            vendor.CreditLimit = model.CreditLimit;
            vendor.ZoneId = model.ZoneId;
            vendor.SubZoneId = model.SubZoneId;
            vendor.DivisionId = model.DivisionId;
            vendor.DistrictId = model.DistrictId;
            vendor.UpazilaId = model.UpazilaId;
            vendor.CustomerType = model.CustomerType;
            vendor.SecurityAmount = model.SecurityAmount;
            vendor.ACName = model.ACName;
            vendor.ACNo = model.ACNo;
            vendor.BankName = model.BankName;
            vendor.BranchName = model.BranchName;
            return vendor;
        }
        public async Task<int> UpdateRecord(VendorViewModel model)
        {
            var user = await workContext.GetCurrentUserAsync();
            var checkphone = await dbContext.Vendor.FirstOrDefaultAsync(f => f.Mobile == model.Mobile && f.TrakingId == user.Id && f.VendorType == model.VendorType&&f.Id!=model.Id);
            if (checkphone == null)
            {
                Vendor vendor = await _entityRepository.GetByIdAsync(model.Id);
                vendor.Name = model.Name;
                vendor.Mobile = model.Mobile;
                vendor.Email = model.Email;
                vendor.Address = model.Address;
                vendor.VendorType = model.VendorType;
                vendor.NID = model.NID;
                vendor.ContactName = model.ContactName;
                vendor.CreditLimit = model.CreditLimit;
                vendor.ZoneId = model.ZoneId;
                vendor.SubZoneId = model.SubZoneId;
                vendor.DivisionId = model.DivisionId;
                vendor.DistrictId = model.DistrictId;
                vendor.UpazilaId = model.UpazilaId;
                vendor.CustomerType = model.CustomerType;
                vendor.PaymentType = model.PaymentType;
                vendor.SecurityAmount = model.SecurityAmount;
                vendor.IsForeign = model.IsForeign;
                await _entityRepository.UpdateAsync(vendor);
                return 2;
            }
            return 1;
        }
    }
}
