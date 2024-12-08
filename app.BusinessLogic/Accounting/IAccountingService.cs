namespace app.Services.Accounting
{
    public interface IAccountingService
    {
        Task<CostCentersViewModel> CostCentersAsync();
        Task<CostCentersViewModel> CostCenterAync(long id);
        Task<ResponseViewModel> AddCostCenterAsync(CostCentersViewModel model);
        Task<ResponseViewModel> UpdateCostCenterAync(CostCentersViewModel model);
        Task<ResponseViewModel> DeleteCostCenterAync(long id);
    }
}
