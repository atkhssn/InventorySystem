using app.EntityModel.CoreModel;
using app.Services.DropDownServices;
using app.Services.Purchase_Return_Service;
using app.Services.PurchaseFinalized_Services;
using app.Services.PurchaseOrder_Services;
using app.Services.SalaesReturn_service;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    public class PurchaseReturnController : Controller
    {
        private readonly IPurchaseReturnService purchaseReturnService;
        private readonly IDropDownService dropDownService;
        private readonly IPurchaseFinalizedServices purchaseFinalizedServices;
        public PurchaseReturnController(IPurchaseReturnService purchaseReturnService,
            IDropDownService dropDownService,
            IPurchaseFinalizedServices purchaseFinalizedServices)
        {
            this.purchaseReturnService = purchaseReturnService;
            this.dropDownService = dropDownService;
            this.purchaseFinalizedServices = purchaseFinalizedServices;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await purchaseReturnService.GetPagedListAsync(page, pagesize, stringsearch);
            return View(results);
        }
        [HttpGet]
        public async Task<ActionResult> GetPaged(int page = 1, int pagesize = 15, string stringsearch = null)
        {
            if (page < 1)
                page = 1;
            var results = await purchaseReturnService.GetPagedListAsync(page, pagesize, stringsearch);
            return PartialView("_PurchaseReturn", results);
        }


        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            var BnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            DateTime BaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, BnTimeZone);
            PurchaseReturnViewModel viewModel = new PurchaseReturnViewModel();
            viewModel.PurchaseReturnDate = BaTime;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(PurchaseReturnViewModel viewModel)
        {
            var result = await purchaseReturnService.AddPurchaseReturn(viewModel);
            if (result > 0)
            {
                return RedirectToAction("Detalis", new { id = result });
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalis(long id)
        {
            var result = await purchaseReturnService.GetPurchaseReturn(id);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> PurchaseReturnDetailReport(long id)
        {
            var result = await purchaseReturnService.GetPurchaseReturn(id);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRecord(long id)
        {
            var result = await purchaseReturnService.GetPurchaseReturn(id);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> DeletepurchaseOrderDetalis(long id)
        {
            var result = await purchaseReturnService.DeletePurchaseReturnDetalies(id);
            return RedirectToAction("UpdateRecord", new { id = result });
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseReturnDetalies(PurchaseReturnDetailsViewModel model)
        {
            var result = await purchaseReturnService.AddPurchaseReturnDetalies(model);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecord(PurchaseReturnViewModel viewModel)
        {
            var result = await purchaseReturnService.UpdatePurchaseReturn(viewModel);
            if (result > 0)
            {
                return RedirectToAction("Detalis", new { id = result });
            }

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> ChangeStatus(long id)
        {
            var result = await purchaseFinalizedServices.GetPurchaseReturnFinalizedAsync(id);
            return RedirectToAction("Index");
        }

    }
}
