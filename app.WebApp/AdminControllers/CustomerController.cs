using app.Services.ProductCategory_Services;
using app.Services.Vendor_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly IVendorService vendorService;
        public CustomerController(IVendorService vendorService)
        {
            this.vendorService = vendorService;
        }
        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await vendorService.GetPagedListAsync(page, pagesize,2, stringsearch);
            return View(results);
        }
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await vendorService.GetPagedListAsync(page, pagesize, 2, stringsearch);
            return PartialView("_customerCategorypartial", results);
        }

        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            VendorViewModel viewModel = new VendorViewModel();
            viewModel.VendorType = 2;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(VendorViewModel viewModel)
        {
             
            var result = await vendorService.AddRecord(viewModel);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await vendorService.DeleteRecord(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRecort(long id)
        {
            VendorViewModel viewModel = new VendorViewModel();
            viewModel = await vendorService.GetRecord(id);
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRecort(VendorViewModel viewModel)
        {
            var result = await vendorService.UpdateRecord(viewModel);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(viewModel);
        }
    }
}
