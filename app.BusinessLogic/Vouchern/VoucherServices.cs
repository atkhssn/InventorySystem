using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using app.Utility;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
            var findVoucherType = await _dbContext.VoucherTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (findVoucherType is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to delete.";
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

        #region 

        public async Task<VouchersViewModel> VouchersAsync()
        {
            VouchersViewModel viewModel = new VouchersViewModel();
            viewModel.Vouchers = await _dbContext.Vouchers.Where(x => x.IsActive).Select(x => new VouchersViewModel
            {
                Id = x.Id,
                VoucherNo = x.VoucherNo,
                VoucherDate = x.VoucherDate,
                VoucherTypesId = x.VoucherTypesId,
                CostCentersId = x.CostCentersId,
                TotalDebitAmount = x.TotalDebitAmount,
                TotalCreditAmount = x.TotalCreditAmount,
                Narration = x.Narration,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = x.UpdatedOn,
                IsActive = x.IsActive,
            }).ToListAsync();

            return viewModel;
        }

        public async Task<VouchersViewModel> VoucherAsync(long Id)
        {
            VouchersViewModel viewModel = new VouchersViewModel();
            viewModel = await _dbContext.Vouchers.Where(x => x.Id == Id && x.IsActive).Select(x => new VouchersViewModel
            {
                Id = x.Id,
                VoucherNo = x.VoucherNo,
                VoucherDate = x.VoucherDate,
                VoucherTypesId = x.VoucherTypesId,
                CostCentersId = x.CostCentersId,
                TotalDebitAmount = x.TotalDebitAmount,
                TotalCreditAmount = x.TotalCreditAmount,
                Narration = x.Narration,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = x.UpdatedOn,
                IsActive = x.IsActive,
            }).FirstOrDefaultAsync();

            return viewModel;
        }

        public async Task<VouchersViewModel> VoucherByVoucherNoAsync(string voucherNo)
        {
            VouchersViewModel viewModel = new VouchersViewModel();
            viewModel = await _dbContext.Vouchers.Where(x => x.VoucherNo.ToUpper() == voucherNo.ToUpper() && x.IsActive).Select(x => new VouchersViewModel
            {
                Id = x.Id,
                VoucherNo = x.VoucherNo,
                VoucherDate = x.VoucherDate,
                VoucherTypesId = x.VoucherTypesId,
                CostCentersId = x.CostCentersId,
                TotalDebitAmount = x.TotalDebitAmount,
                TotalCreditAmount = x.TotalCreditAmount,
                Narration = x.Narration,
                TrakingId = x.TrakingId,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                UpdatedBy = x.UpdatedBy,
                UpdatedOn = x.UpdatedOn,
                IsActive = x.IsActive,
            }).FirstOrDefaultAsync();

            return viewModel;
        }

        public async Task<VouchersViewModel> AddVoucherAsync(VouchersViewModel model)
        {
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucherType = await _dbContext.VoucherTypes.Where(x => x.Id == model.VoucherTypesId && x.IsActive).FirstOrDefaultAsync();
            var findCostCenter = await _dbContext.CostCenters.Where(x => x.Id == model.CostCentersId && x.IsActive).FirstOrDefaultAsync();

            if (findVoucherType is null)
            {
                model.ResponseViewModel.ResponseCode = 404;
                model.ResponseViewModel.ResponseMessage = $"Voucher type is not found.";
                return await Task.Run(() => model);
            }

            if (findCostCenter is null)
            {
                model.ResponseViewModel.ResponseCode = 500;
                model.ResponseViewModel.ResponseMessage = $"An internal server error occurred.";
                return await Task.Run(() => model);
            }

            model.VoucherNo = await VoucherNoGenerate(model.VoucherTypesId, model.VoucherDate);
            if (string.IsNullOrEmpty(model.VoucherNo))
            {
                model.ResponseViewModel.ResponseCode = 500;
                model.ResponseViewModel.ResponseMessage = $"Something went wrong. Please try again.";
                return await Task.Run(() => model);
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Vouchers vouchers = new Vouchers
                    {
                        Id = model.Id,
                        VoucherNo = model.VoucherNo,
                        VoucherDate = model.VoucherDate,
                        VoucherTypesId = model.VoucherTypesId,
                        CostCentersId = model.CostCentersId,
                        TotalDebitAmount = model.VouchersLinesViewModel.DebitAmount,
                        TotalCreditAmount = model.VouchersLinesViewModel.CreditAmount,
                        Narration = model.Narration,
                        Status = Convert.ToInt32(VoucherStatus.Created),
                        TrakingId = user.UserName,
                        CreatedBy = user.FullName,
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    _dbContext.Vouchers.Add(vouchers);

                    VouchersLines vouchersLines = new VouchersLines
                    {
                        VouchersId = model.Id,
                        GlHeadId = model.VouchersLinesViewModel.GlHeadId,
                        DebitAmount = model.VouchersLinesViewModel.DebitAmount,
                        CeditAmount = model.VouchersLinesViewModel.CreditAmount,
                        Particular = model.VouchersLinesViewModel.Particular,
                        TrakingId = user.UserName,
                        CreatedBy = user.FullName,
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    _dbContext.VouchersLines.Add(vouchersLines);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        model.VouchersLinesViewModels.Add(model.VouchersLinesViewModel);
                        model.VouchersLinesViewModel = new VouchersLinesViewModel();
                        model.ResponseViewModel.ResponseCode = 200;
                        model.ResponseViewModel.ResponseMessage = "New voucher has been added.";
                        transaction.Complete();
                    }
                    else
                    {
                        model.ResponseViewModel.ResponseCode = 500;
                        model.ResponseViewModel.ResponseMessage = "An internal server error occurred.";
                    }

                    return model;
                }

            }
            catch (Exception ex)
            {
                model.ResponseViewModel.ResponseCode = 500;
                model.ResponseViewModel.ResponseMessage = ex.Message.ToString();
                return await Task.Run(() => model);
            }
        }

        public async Task<ResponseViewModel> DeleteVoucherAync(long Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucher = await _dbContext.Vouchers.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (findVoucher is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to delete.";
                return await Task.Run(() => response);
            }

            if (Convert.ToInt32(VoucherStatus.Created).Equals(findVoucher.Status))
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"Approved vouchers cannot be deleted.";
                return await Task.Run(() => response);
            }

            findVoucher.IsActive = false;
            findVoucher.UpdatedBy = user.FullName;
            findVoucher.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"Voucher has been removed.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        public Task<VouchersLinesViewModel> VoucherLinesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<VouchersLinesViewModel> VoucherLineAsync(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<VouchersLinesViewModel> VoucherLinesByVoucherIdAsync(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<VouchersViewModel> AddVoucherLineAsync(VouchersViewModel model)
        {
            var user = await _workContext.GetCurrentUserAsync();
            var getVoucher = await _dbContext.Vouchers.Where(x => x.Id.Equals(model.Id) && x.IsActive).FirstOrDefaultAsync();

            if (getVoucher is null)
            {
                model.ResponseViewModel.ResponseCode = 404;
                model.ResponseViewModel.ResponseMessage = $"Voucher is not found.";
                return await Task.Run(() => model);
            }

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    VouchersLines vouchersLines = new VouchersLines
                    {
                        VouchersId = model.Id,
                        GlHeadId = model.VouchersLinesViewModel.GlHeadId,
                        DebitAmount = model.VouchersLinesViewModel.DebitAmount,
                        CeditAmount = model.VouchersLinesViewModel.CreditAmount,
                        Particular = model.VouchersLinesViewModel.Particular,
                        TrakingId = user.UserName,
                        CreatedBy = user.FullName,
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    _dbContext.VouchersLines.Add(vouchersLines);

                    getVoucher.TotalDebitAmount += model.VouchersLinesViewModel.DebitAmount;
                    getVoucher.TotalCreditAmount += model.VouchersLinesViewModel.CreditAmount;

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        model.VouchersLinesViewModels.Add(model.VouchersLinesViewModel);
                        model.VouchersLinesViewModel = new VouchersLinesViewModel();
                        model.ResponseViewModel.ResponseCode = 200;
                        model.ResponseViewModel.ResponseMessage = "New voucher detail has been added.";
                        transaction.Complete();
                    }
                    else
                    {
                        model.ResponseViewModel.ResponseCode = 500;
                        model.ResponseViewModel.ResponseMessage = "An internal server error occurred.";
                    }

                    return model;
                }

            }
            catch (Exception ex)
            {
                model.ResponseViewModel.ResponseCode = 500;
                model.ResponseViewModel.ResponseMessage = ex.Message.ToString();
                return await Task.Run(() => model);
            }
        }

        public async Task<ResponseViewModel> SubmitVoucherAync(long Id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucher = await _dbContext.Vouchers.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (findVoucher is null)
            {
                response.ResponseCode = 404;
                response.ResponseMessage = $"No records found to submit.";
                return await Task.Run(() => response);
            }

            if (Convert.ToInt32(VoucherStatus.Created).Equals(findVoucher.Status))
            {
                response.ResponseCode = 400;
                response.ResponseMessage = $"Approved vouchers cannot be submitted.";
                return await Task.Run(() => response);
            }

            findVoucher.Status = Convert.ToInt32(VoucherStatus.Submitted);
            findVoucher.UpdatedBy = user.FullName;
            findVoucher.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                response.ResponseCode = 200;
                response.ResponseMessage = $"Voucher has been submited.";
                return await Task.Run(() => response);
            }

            response.ResponseCode = 500;
            response.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => response);
        }

        #endregion

        public async Task<string> VoucherNoGenerate(long voucherTypeId, DateTime voucherDate)
        {
            string voucherNo = string.Empty;
            var getVouchers = await _dbContext.Vouchers.Where(x => x.Id.Equals(voucherTypeId) && x.VoucherDate.Equals(voucherDate)).ToListAsync();
            var getVoucherType = await _dbContext.VoucherTypes.Where(x => x.Id.Equals(voucherTypeId)).FirstOrDefaultAsync();
            voucherNo = $"{getVoucherType.ShortName}{getVoucherType.Id}0{voucherDate.Year % 100}{voucherDate.Month}{voucherDate.Day}0{getVouchers.Count + 1}";
            return await Task.Run(() => voucherNo);
        }

    }
}
