using app.Services.Stock_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.WebApp.AdminControllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly IStockService stockService;
        public StockController(IStockService stockService)
        {
            this.stockService = stockService;
        }
        public async Task<IActionResult> Index()
        {
            var result=await stockService.GetStocks();  
            return View(result);
        }
    }
}
