namespace app.Services.Accounting
{
    public interface IAccountingService
    {
        Task<CostCenterViewModel> CostCentersAsync();
        Task<CostCenterViewModel> CostCenterAync(long Id);
        Task<ResponseViewModel> AddCostCenterAsync(CostCenterViewModel model);
        Task<ResponseViewModel> UpdateCostCenterAync(CostCenterViewModel model);
        Task<ResponseViewModel> DeleteCostCenterAync(long Id);
    }
}
