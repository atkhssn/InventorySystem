using app.Services;
using app.Services.Accounting;
using app.Services.Vouchern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class VoucherController : Controller
    {
        private readonly IVoucherServices _voucherServices;
        private readonly IAccountingService _accoutingServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedFileExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".pdf" };

        public VoucherController(IVoucherServices voucherServices, IAccountingService accountingService, IWebHostEnvironment webHostEnvironment)
        {
            _voucherServices = voucherServices;
            _accoutingServices = accountingService;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Voucher type

        [HttpGet]
        public async Task<IActionResult> VoucherTypes()
        {
            var response = await _voucherServices.VoucherTypesAsync();
            ViewBag.Response = TempData["Response"]?.ToString();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> AddVoucherType()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucherType(VoucherTypesViewModel voucherTypesViewModel)
        {
            var response = await _voucherServices.AddVoucherTypeAsync(voucherTypesViewModel);
            if (response.ResponseCode.Equals(200))
            {
                TempData["Response"] = JsonConvert.SerializeObject(response);
                return await Task.Run(() => RedirectToAction("VoucherTypes"));
            }
            voucherTypesViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(voucherTypesViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVoucherType(long id)
        {
            var response = await _voucherServices.VoucherTypeAsync(id);
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVoucherType(VoucherTypesViewModel voucherTypesViewModel)
        {
            var response = await _voucherServices.UpdateVoucherTypeAync(voucherTypesViewModel);
            if (response.ResponseCode.Equals(200))
            {
                TempData["Response"] = JsonConvert.SerializeObject(response);
                return await Task.Run(() => RedirectToAction("VoucherTypes"));
            }
            voucherTypesViewModel.ResponseViewModel = response;
            return await Task.Run(() => View(voucherTypesViewModel));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCostCenter(long id)
        {
            var response = await _voucherServices.DeleteVoucherTypeAync(id);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("VoucherTypes"));
        }

        #endregion

        #region Voucher

        [HttpGet]
        public async Task<IActionResult> Vouchers()
        {
            var response = await _voucherServices.VouchersAsync();
            ViewBag.Response = TempData["Response"]?.ToString();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> DetailVoucher(long id)
        {
            var response = new VouchersViewModel();
            response = await _voucherServices.VoucherAsync(id);
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> AddVoucher(long id = 0)
        {
            var response = new VouchersViewModel();
            if (id.Equals(0))
            {
                response.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
                response.CostCenterViewModel = await _accoutingServices.CostCentersAsync();
            }
            else
            {
                response = await _voucherServices.VoucherAsync(id);
                if (response is null)
                {
                    var nullResponse = new VouchersViewModel
                    {
                        VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync(),
                        CostCenterViewModel = await _accoutingServices.CostCentersAsync(),
                        ResponseViewModel = new ResponseViewModel
                        {
                            ResponseCode = 404,
                            ResponseMessage = "Nothing found or Invalid Voucher Id."
                        }
                    };
                    TempData["Response"] = JsonConvert.SerializeObject(nullResponse.ResponseViewModel);
                    return await Task.Run(() => RedirectToAction("AddVoucher", new { id = 0 }));
                }
            }
            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> AddVoucher(VouchersViewModel vouchersViewModel)
        {
            IFormFile file = vouchersViewModel.VouchersLinesViewModel.Attachment;
            if (file is not null)
            {
                string fileExtention = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedFileExtensions.Contains(fileExtention))
                {
                    vouchersViewModel.ResponseViewModel.ResponseCode = 400;
                    vouchersViewModel.ResponseViewModel.ResponseMessage = $"Invalid file extention {fileExtention}.";
                    return await Task.Run(() => View(vouchersViewModel));
                }

                if (file.Length >= 5 * 1024 * 1024)
                {
                    vouchersViewModel.ResponseViewModel.ResponseCode = 400;
                    vouchersViewModel.ResponseViewModel.ResponseMessage = $"Maximum allowed file size 5MB.";
                    return await Task.Run(() => View(vouchersViewModel));
                }

                try
                {
                    string rootPath = _webHostEnvironment.WebRootPath;
                    string folderPath = "Uploads/VoucherAttachments";
                    string uploadPath = Path.Combine(rootPath, "Uploads", "VoucherAttachments");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var documentName = $"V{vouchersViewModel.VouchersLinesViewModel.AccountCode}{DateTime.Now.ToString("yyMMddHHmmss")}{fileExtention}";
                    var filePath = Path.Combine(uploadPath, documentName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    vouchersViewModel.ImageUrl = $"{folderPath}/{documentName}";
                }
                catch (Exception ex)
                {
                    vouchersViewModel.ResponseViewModel.ResponseCode = 500;
                    vouchersViewModel.ResponseViewModel.ResponseMessage = ex.Message.ToString();
                    return await Task.Run(() => View(vouchersViewModel));
                }
            }

            if (vouchersViewModel.Id.Equals(0))
            {
                var response = await _voucherServices.AddVoucherAsync(vouchersViewModel);
                TempData["Response"] = JsonConvert.SerializeObject(response.ResponseViewModel);
                return await Task.Run(() => RedirectToAction("AddVoucher", new { Id = response.Id }));
            }
            else
            {
                var response = await _voucherServices.AddVoucherLineAsync(vouchersViewModel);
                TempData["Response"] = JsonConvert.SerializeObject(response.ResponseViewModel);
                return await Task.Run(() => RedirectToAction("AddVoucher", new { Id = response.Id }));
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchVouchers(long? voucherType, long? costCenter, string fromDate, string toDate)
        {
            var response = new SearchVoucherViewModel
            {

                FromDate = fromDate is null ? null : Convert.ToDateTime(fromDate),
                ToDate = toDate is null ? null : Convert.ToDateTime(toDate),
                VoucherTypesId = voucherType,
                CostCentersId = costCenter
            };

            if (response.VoucherTypesId.HasValue || response.CostCentersId.HasValue || response.FromDate.HasValue || response.ToDate.HasValue)
            {
                response = await _voucherServices.SearchVoucherAsync(response);
            }

            response.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
            response.CostCenterViewModel = await _accoutingServices.CostCentersAsync();

            return await Task.Run(() => View(response));
        }

        [HttpPost]
        public async Task<IActionResult> SearchVouchers(SearchVoucherViewModel searchVoucherViewModel)
        {
            return await Task.Run(() => RedirectToAction("SearchVouchers", new
            {
                voucherType = searchVoucherViewModel.VoucherTypesId is null ? null : searchVoucherViewModel.VoucherTypesId,
                costCenter = searchVoucherViewModel.CostCentersId is null ? null : searchVoucherViewModel.CostCentersId,
                fromDate = searchVoucherViewModel.FromDate is null ? null : searchVoucherViewModel.FromDate,
                toDate = searchVoucherViewModel.ToDate is null ? null : searchVoucherViewModel.ToDate
            }));
        }

        [HttpPost]
        public async Task<IActionResult> SubmitVoucher(VouchersViewModel vouchersViewModel)
        {
            var response = await _voucherServices.MakeSubmitVoucherAync(vouchersViewModel.Id);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("Vouchers"));
        }

        [HttpGet]
        public async Task<IActionResult> PendingVouchers()
        {
            var response = await _voucherServices.UnapprovedVoucherList();
            return await Task.Run(() => View(response));
        }

        [HttpGet]
        public async Task<IActionResult> ApprovedVoucher(string voucherNo)
        {
            var response = await _voucherServices.MakeApproveVoucherAsync(voucherNo);
            TempData["Response"] = JsonConvert.SerializeObject(response);
            return await Task.Run(() => RedirectToAction("Vouchers"));
        }

        #endregion

        #region Specific Voucher Form

        [HttpGet]
        public async Task<IActionResult> AddBillVoucher(long id = 0)
        {
            var response = new VouchersViewModel();
            if (id.Equals(0))
            {
                response.VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync();
                response.CostCenterViewModel = await _accoutingServices.CostCentersAsync();
            }
            else
            {
                response = await _voucherServices.VoucherAsync(id);
                if (response is null)
                {
                    var nullResponse = new VouchersViewModel
                    {
                        VoucherTypesViewModel = await _voucherServices.VoucherTypesAsync(),
                        CostCenterViewModel = await _accoutingServices.CostCentersAsync(),
                        ResponseViewModel = new ResponseViewModel
                        {
                            ResponseCode = 404,
                            ResponseMessage = "Nothing found or Invalid Voucher Id."
                        }
                    };
                    TempData["Response"] = JsonConvert.SerializeObject(nullResponse.ResponseViewModel);
                    return await Task.Run(() => RedirectToAction("AddVoucher", new { id = 0 }));
                }
            }
            return await Task.Run(() => View(response));
        }

        #endregion

    }
}
