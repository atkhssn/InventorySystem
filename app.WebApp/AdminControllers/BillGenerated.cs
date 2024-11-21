using app.EntityModel.CoreModel;
using app.Services.BillGenerateds;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    public class BillGenerated : Controller
    {
        private readonly IBillGeneratedService billGeneratedService;
        public BillGenerated(IBillGeneratedService billGeneratedService)
        {
            this.billGeneratedService = billGeneratedService;
        }
        public async Task<IActionResult> Index()
        {
            var res = await billGeneratedService.GetAllRecort();
            return View(res);
        }
        [HttpGet]
        public async Task<IActionResult> NewBillGenerated()
        {
            return View();
        }
        [HttpPost] 
        public async Task<IActionResult> NewBillGenerated(BillGeneratedViewModel model)
        {
            var res = await billGeneratedService.AddRecort(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await billGeneratedService.DeleteRecort(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> PayNow(long id)
        {
            var res = await billGeneratedService.GetByRecort(id);
            return View(res);
        }
    }
}
