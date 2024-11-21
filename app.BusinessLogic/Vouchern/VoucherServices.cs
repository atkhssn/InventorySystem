using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;

namespace app.Services.Vouchern
{
    public class VoucherServices : IVoucherServices
    {
        private readonly inventoryDbContext _dbContext;
        private readonly IWorkContext _workContext;
        public VoucherServices(IWorkContext workContext, inventoryDbContext dbContext)
        {
            _dbContext = dbContext;
            _workContext = workContext;
        }

        #region Voucher Type

        public async Task<VoucherTypesViewModel> VoucherTypesAsync()
        {
            VoucherTypesViewModel viewModel = new VoucherTypesViewModel();
            viewModel.VoucherTypesViewModels = await _dbContext.VoucherTypes.Where(x => x.IsActive).Select(x => new VoucherTypesViewModel
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Name = x.Name,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = DateTime.Now,
                IsActive = true
            }).ToListAsync();
            return viewModel;
        }

        public async Task<VoucherTypesViewModel> VoucherTypeAsync(long Id)
        {
            VoucherTypesViewModel viewModel = new VoucherTypesViewModel();
            viewModel = await _dbContext.VoucherTypes.Where(x => x.Id == Id && x.IsActive).Select(x => new VoucherTypesViewModel
            {
                Id = x.Id,
                ShortName = x.ShortName,
                Name = x.Name,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = DateTime.Now,
                IsActive = true
            }).FirstOrDefaultAsync();
            return viewModel;
        }

        public async Task<ResponseViewModel> AddVoucherTypeAsync(VoucherTypesViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVouchertype = _dbContext.VoucherTypes.Where(x => x.Name.ToUpper() == model.Name.ToUpper()).FirstOrDefault();
            var findVouchertypeShort = _dbContext.VoucherTypes.Where(x => x.ShortName.ToUpper() == model.ShortName.ToUpper()).FirstOrDefault();

            if (findVouchertype is not null)
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => response);
            }

            if (findVouchertypeShort is not null)
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"[{model.ShortName}] has already been added.";
                return await Task.Run(() => response);
            }

            VoucherTypes voucherTypes = new VoucherTypes
            {
                Id = model.Id,
                ShortName = model.ShortName.ToUpper(),
                Name = model.Name,
                TrakingId = user.UserName,
                CreatedBy = user.FullName,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _dbContext.VoucherTypes.Add(voucherTypes);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"New voucher type has been added.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        public async Task<ResponseViewModel> UpdateVoucherTypeAync(VoucherTypesViewModel model)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var getVoucherType = _dbContext.VoucherTypes.Where(x => x.Id == model.Id && x.ShortName.ToUpper() == model.ShortName.ToUpper()).FirstOrDefault();
            var findVoucherType = _dbContext.VoucherTypes.Where(x => x.Name.ToUpper() == model.Name.ToUpper()).FirstOrDefault();

            if (getVoucherType is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => response);
            }

            if (findVoucherType is not null)
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => response);
            }

            getVoucherType.Name = model.Name;
            getVoucherType.UpdatedBy = user.FullName;
            getVoucherType.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"Voucher type has been modified.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        public async Task<ResponseViewModel> DeleteVoucherTypeAync(long Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucherType = _dbContext.VoucherTypes.Where(x => x.Id == Id).FirstOrDefault();

            if (findVoucherType is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => response);
            }

            findVoucherType.IsActive = false;
            findVoucherType.UpdatedBy = user.FullName;
            findVoucherType.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"Voucher type has been removed.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        #endregion
    }
}
