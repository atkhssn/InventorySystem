using app.EntityModel.CoreModel;
using app.Infrastructure;
using app.Infrastructure.Auth;
using app.Services.Accounting;
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
            VoucherTypesViewModel request = new VoucherTypesViewModel();
            request.VoucherTypesViewModels = await _dbContext.VoucherTypes.Where(x => x.IsActive)
                .Select(x => new VoucherTypesViewModel
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
            return request;
        }

        public async Task<VoucherTypesViewModel> VoucherTypeAsync(long id)
        {
            VoucherTypesViewModel request = new VoucherTypesViewModel();
            request = await _dbContext.VoucherTypes.Where(x => x.Id.Equals(id) && x.IsActive)
                .Select(x => new VoucherTypesViewModel
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
            return request;
        }

        public async Task<ResponseViewModel> AddVoucherTypeAsync(VoucherTypesViewModel model)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVouchertype = _dbContext.VoucherTypes.Where(x => x.Name.ToUpper().Equals(model.Name.ToUpper())).FirstOrDefault();
            var findVouchertypeShort = _dbContext.VoucherTypes.Where(x => x.ShortName.ToUpper().Equals(model.ShortName.ToUpper())).FirstOrDefault();

            if (findVouchertype is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => request);
            }

            if (findVouchertypeShort is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.ShortName}] has already been added.";
                return await Task.Run(() => request);
            }

            VoucherTypes voucherType = new VoucherTypes
            {
                Id = model.Id,
                ShortName = model.ShortName.ToUpper(),
                Name = model.Name,
                TrakingId = user.UserName,
                CreatedBy = user.FullName,
                CreatedOn = DateTime.Now,
                IsActive = true
            };

            _dbContext.VoucherTypes.Add(voucherType);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"New voucher type has been added.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> UpdateVoucherTypeAync(VoucherTypesViewModel model)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var getVoucherType = _dbContext.VoucherTypes.Where(x => x.Id.Equals(model.Id) && x.ShortName.ToUpper().Equals(model.ShortName.ToUpper())).FirstOrDefault();
            var findVoucherType = _dbContext.VoucherTypes.Where(x => x.Name.ToUpper().Equals(model.Name.ToUpper())).FirstOrDefault();

            if (getVoucherType is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to update.";
                return await Task.Run(() => request);
            }

            if (findVoucherType is not null)
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"[{model.Name}] has already been added.";
                return await Task.Run(() => request);
            }

            getVoucherType.Name = model.Name;
            getVoucherType.UpdatedBy = user.FullName;
            getVoucherType.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"Voucher type has been modified.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> DeleteVoucherTypeAync(long id)
        {
            ResponseViewModel response = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucherType = await _dbContext.VoucherTypes.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

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

        #region Voucher

        public async Task<VouchersViewModel> VouchersAsync()
        {
            VouchersViewModel request = new VouchersViewModel();
            request.VouchersViewModels = await _dbContext.Vouchers
                .Where(x => x.IsActive)
                .Include(x => x.VoucherTypes)
                .Include(x => x.CostCenters)
                .Select(x => new VouchersViewModel
                {
                    Id = x.Id,
                    VoucherNo = x.VoucherNo,
                    VoucherDate = x.VoucherDate,
                    VoucherTypesId = x.VoucherTypesId,
                    VoucherTypesViewModel = new VoucherTypesViewModel
                    {
                        Id = x.VoucherTypes.Id,
                        Name = x.VoucherTypes.Name,
                    },
                    CostCentersId = x.CostCentersId,
                    CostCenterViewModel = new CostCentersViewModel
                    {
                        Id = x.CostCenters.Id,
                        Name = x.CostCenters.Name,
                    },
                    TotalDebitAmount = x.TotalDebitAmount,
                    TotalCreditAmount = x.TotalCreditAmount,
                    Narration = x.Narration,
                    VoucherStatus = (VoucherStatus)x.Status,
                }).ToListAsync();

            return request;
        }

        public async Task<VouchersViewModel> VoucherAsync(long id)
        {
            VouchersViewModel request = await _dbContext.Vouchers
            .Include(v => v.VoucherTypes)
            .Include(v => v.CostCenters)
            .Where(v => v.Id.Equals(id) && v.IsActive)
            .Select(v => new VouchersViewModel
            {
                Id = v.Id,
                VoucherNo = v.VoucherNo,
                VoucherDate = v.VoucherDate,
                VoucherTypesId = v.VoucherTypesId,
                VoucherTypesViewModel = new VoucherTypesViewModel
                {
                    Id = v.VoucherTypes.Id,
                    Name = v.VoucherTypes.Name,
                    ShortName = v.VoucherTypes.ShortName,
                    TrakingId = v.VoucherTypes.TrakingId,
                    CreatedBy = v.VoucherTypes.CreatedBy,
                    CreatedOn = v.VoucherTypes.CreatedOn,
                    UpdatedBy = v.VoucherTypes.UpdatedBy,
                    UpdatedOn = v.VoucherTypes.UpdatedOn,
                    IsActive = v.VoucherTypes.IsActive
                },
                CostCentersId = v.CostCentersId,
                CostCenterViewModel = new CostCentersViewModel
                {
                    Id = v.CostCenters.Id,
                    Name = v.CostCenters.Name,
                    ShortName = v.CostCenters.ShortName,
                    TrakingId = v.CostCenters.TrakingId,
                    CreatedBy = v.CostCenters.CreatedBy,
                    CreatedOn = v.CostCenters.CreatedOn,
                    UpdatedBy = v.CostCenters.UpdatedBy,
                    UpdatedOn = v.CostCenters.UpdatedOn,
                    IsActive = v.CostCenters.IsActive
                },
                TotalDebitAmount = v.TotalDebitAmount,
                TotalCreditAmount = v.TotalCreditAmount,
                Narration = v.Narration,
                VoucherStatus = (VoucherStatus)v.Status,
                CreatedBy = v.CreatedBy,
                CreatedOn = v.CreatedOn,
                UpdatedBy = v.UpdatedBy,
                UpdatedOn = v.UpdatedOn,
                IsActive = v.IsActive
            }).FirstOrDefaultAsync();

            request.VouchersLinesViewModels = await _dbContext.VouchersLines
                .Where(x => x.VouchersId.Equals(request.Id) && x.IsActive).Select(x => new VouchersLinesViewModel
                {
                    Id = x.Id,
                    VouchersId = x.VouchersId,
                    GlHeadId = x.GlHeadId,
                    DebitAmount = x.DebitAmount,
                    CreditAmount = x.CreditAmount,
                    Particular = x.Particular,
                    TrakingId = x.TrakingId,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn,
                    IsActive = x.IsActive
                }).ToListAsync();

            return request;
        }

        public async Task<VouchersViewModel> VoucherByVoucherNoAsync(string voucherNo)
        {
            VouchersViewModel request = new VouchersViewModel();
            request = await _dbContext.Vouchers
                .Where(x => x.VoucherNo.Equals(voucherNo) && x.Status.Equals(VoucherStatus.SUBMITTED) && x.IsActive)
                .Select(x => new VouchersViewModel
                {
                    Id = x.Id,
                    VoucherNo = x.VoucherNo,
                    VoucherDate = x.VoucherDate,
                    VoucherTypesId = x.VoucherTypesId,
                    CostCentersId = x.CostCentersId,
                    TotalDebitAmount = x.TotalDebitAmount,
                    TotalCreditAmount = x.TotalCreditAmount,
                    VoucherStatus = (VoucherStatus)x.Status,
                    Narration = x.Narration,
                    TrakingId = x.TrakingId,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedOn = x.UpdatedOn,
                    IsActive = x.IsActive,
                }).FirstOrDefaultAsync();

            return request;
        }

        public async Task<VouchersViewModel> UnapprovedVoucherList()
        {
            VouchersViewModel request = new VouchersViewModel();
            request.VouchersViewModels = await _dbContext.Vouchers
                .Where(x => x.Status.Equals((int)VoucherStatus.SUBMITTED) && x.IsActive)
                .Include(x => x.VoucherTypes)
                .Include(x => x.CostCenters)
                .Select(x => new VouchersViewModel
                {
                    Id = x.Id,
                    VoucherNo = x.VoucherNo,
                    VoucherDate = x.VoucherDate,
                    VoucherTypesId = x.VoucherTypesId,
                    VoucherTypesViewModel = new VoucherTypesViewModel
                    {
                        Id = x.VoucherTypes.Id,
                        Name = x.VoucherTypes.Name,
                    },
                    CostCentersId = x.CostCentersId,
                    CostCenterViewModel = new CostCentersViewModel
                    {
                        Id = x.CostCenters.Id,
                        Name = x.CostCenters.Name,
                    },
                    TotalDebitAmount = x.TotalDebitAmount,
                    TotalCreditAmount = x.TotalCreditAmount,
                    Narration = x.Narration,
                    VoucherStatus = (VoucherStatus)x.Status,
                }).ToListAsync();

            return request;
        }

        public async Task<VouchersViewModel> AddVoucherAsync(VouchersViewModel model)
        {
            model.ResponseViewModel = new ResponseViewModel();
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
                        Status = Convert.ToInt32(VoucherStatus.CREATED),
                        TrakingId = user.UserName,
                        CreatedBy = user.FullName,
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    _dbContext.Vouchers.Add(vouchers);
                    await _dbContext.SaveChangesAsync();

                    VouchersLines vouchersLines = new VouchersLines
                    {
                        VouchersId = vouchers.Id,
                        GlHeadId = model.VouchersLinesViewModel.GlHeadId,
                        DebitAmount = model.VouchersLinesViewModel.DebitAmount,
                        CreditAmount = model.VouchersLinesViewModel.CreditAmount,
                        Particular = model.VouchersLinesViewModel.Particular,
                        TrakingId = user.UserName,
                        CreatedBy = user.FullName,
                        CreatedOn = DateTime.Now,
                        IsActive = true
                    };

                    _dbContext.VouchersLines.Add(vouchersLines);

                    if (await _dbContext.SaveChangesAsync() > 0)
                    {
                        model.Id = vouchers.Id;
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

        public async Task<ResponseViewModel> DeleteVoucherAync(long id)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucher = await _dbContext.Vouchers.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if (findVoucher is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to delete.";
                return await Task.Run(() => request);
            }

            if (Convert.ToInt32(VoucherStatus.CREATED).Equals(findVoucher.Status))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Approved vouchers cannot be deleted.";
                return await Task.Run(() => request);
            }

            findVoucher.IsActive = false;
            findVoucher.UpdatedBy = user.FullName;
            findVoucher.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"Voucher has been removed.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<VouchersViewModel> AddVoucherLineAsync(VouchersViewModel model)
        {
            model.ResponseViewModel = new ResponseViewModel();
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
                        CreditAmount = model.VouchersLinesViewModel.CreditAmount,
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

        public async Task<ResponseViewModel> MakeSubmitVoucherAync(long id)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucher = await _dbContext.Vouchers.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if (findVoucher is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to submit.";
                return await Task.Run(() => request);
            }

            if (!findVoucher.Status.Equals(Convert.ToInt32(VoucherStatus.CREATED)))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Voucher is already submitted or approved.";
                return await Task.Run(() => request);
            }

            if (!findVoucher.TotalDebitAmount.Equals(findVoucher.TotalCreditAmount))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Debit amount and creadit amount is mismatched.";
                return await Task.Run(() => request);
            }

            findVoucher.Status = Convert.ToInt32(VoucherStatus.SUBMITTED);
            findVoucher.UpdatedBy = user.FullName;
            findVoucher.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"Voucher has been submited.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<ResponseViewModel> MakeApproveVoucherAsync(string voucherString)
        {
            ResponseViewModel request = new ResponseViewModel();
            var user = await _workContext.GetCurrentUserAsync();
            var findVoucher = await _dbContext.Vouchers.Where(x => x.VoucherNo.Equals(voucherString)).FirstOrDefaultAsync();

            if (findVoucher is null)
            {
                request.ResponseCode = 404;
                request.ResponseMessage = $"No records found to submit.";
                return await Task.Run(() => request);
            }

            if (!findVoucher.Status.Equals(Convert.ToInt32(VoucherStatus.SUBMITTED)))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Voucher is not submitted.";
                return await Task.Run(() => request);
            }

            if (findVoucher.Status.Equals(Convert.ToInt32(VoucherStatus.APPROVED)))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Voucher is already approved.";
                return await Task.Run(() => request);
            }

            if (!findVoucher.TotalDebitAmount.Equals(findVoucher.TotalCreditAmount))
            {
                request.ResponseCode = 400;
                request.ResponseMessage = $"Debit amount and creadit amount is mismatched.";
                return await Task.Run(() => request);
            }

            findVoucher.Status = Convert.ToInt32(VoucherStatus.APPROVED);
            findVoucher.UpdatedBy = user.FullName;
            findVoucher.UpdatedOn = DateTime.Now;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                request.ResponseCode = 200;
                request.ResponseMessage = $"Voucher has been approved.";
                return await Task.Run(() => request);
            }

            request.ResponseCode = 500;
            request.ResponseMessage = $"An internal server error occurred.";
            return await Task.Run(() => request);
        }

        public async Task<SearchVoucherViewModel> SearchVoucherAsync(SearchVoucherViewModel model)
        {
            SearchVoucherViewModel request = new SearchVoucherViewModel();
            var query = _dbContext.Vouchers.AsQueryable();

            if (model.VoucherTypesId.HasValue)
            {
                query = query.Where(v => v.VoucherTypesId.Equals(model.VoucherTypesId));
            }

            if (model.CostCentersId.HasValue)
            {
                query = query.Where(v => v.CostCentersId.Equals(model.CostCentersId));
            }

            if (model.FromDate.HasValue)
            {
                query = query.Where(v => v.VoucherDate.Date >= model.FromDate.Value.Date);
            }

            if (model.ToDate.HasValue)
            {
                query = query.Where(v => v.VoucherDate.Date <= model.ToDate.Value.Date);
            }

            request.VouchersViewModels = await query
                .Where(x => x.IsActive)
                .Include(x => x.VoucherTypes)
                .Include(x => x.CostCenters)
                .Select(x => new VouchersViewModel
                {
                    Id = x.Id,
                    VoucherNo = x.VoucherNo,
                    VoucherDate = x.VoucherDate,
                    VoucherTypesId = x.VoucherTypesId,
                    VoucherTypesViewModel = new VoucherTypesViewModel
                    {
                        Id = x.VoucherTypes.Id,
                        Name = x.VoucherTypes.Name,
                    },
                    CostCentersId = x.CostCentersId,
                    CostCenterViewModel = new CostCentersViewModel
                    {
                        Id = x.CostCenters.Id,
                        Name = x.CostCenters.Name,
                    },
                    TotalDebitAmount = x.TotalDebitAmount,
                    TotalCreditAmount = x.TotalCreditAmount,
                    Narration = x.Narration,
                    VoucherStatus = (VoucherStatus)x.Status,
                }).ToListAsync();

            return request;
        }

        #endregion

        public async Task<string> VoucherNoGenerate(long voucherTypeId, DateTime voucherDate)
        {
            string voucherNo = string.Empty;
            var getVouchers = await _dbContext.Vouchers
                .Where(x => x.VoucherTypesId.Equals(voucherTypeId) && x.VoucherDate.Equals(voucherDate.Date)).ToListAsync();
            var getVoucherType = await _dbContext.VoucherTypes.Where(x => x.Id.Equals(voucherTypeId)).FirstOrDefaultAsync();
            voucherNo = $"{getVoucherType.ShortName}{getVoucherType.Id}0{voucherDate.Year % 100}{voucherDate.Month}{voucherDate.Day}0{getVouchers.Count + 1}";
            return await Task.Run(() => voucherNo);
        }
    }
}
