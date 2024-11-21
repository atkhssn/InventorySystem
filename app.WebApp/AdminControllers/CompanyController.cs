using app.Services.Company_Service;
using app.Services.ProductCategory_Services;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    public class CompanyController : Controller
    {
        public readonly ICompanyService companyService;
        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }
        public async Task<IActionResult> Index()
        {
            var result= await companyService.GetAllRecord();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> AddRecord()
        {
            CompanyViewModel viewModel = new CompanyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(CompanyViewModel viewModel)
        {
            var result = await companyService.AddRecord(viewModel);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateRecord(long id)
        {
            var result = await companyService.GetRecord(id);
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRecord(CompanyViewModel model)
        {
            var result= await companyService.UpdateRecord(model);
            if (result == 2)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Same Name already exists!");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            var res = await companyService.DeleteRecord(id);
            return RedirectToAction("Index");
        }


    }
}
