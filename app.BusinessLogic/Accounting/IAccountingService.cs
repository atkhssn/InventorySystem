namespace app.Services.Accounting
{
    public interface IAccountingService
    {
        Task<CostCenterViewModel> GetAllRecordAsync();
        Task<CostCenterViewModel> GetRecordDetailAync(long Id);
        Task<bool> AddRecordAsync(CostCenterViewModel model);
        Task<bool> UpdateRecordAync(CostCenterViewModel model);
        Task<bool> DeleteRecordAync(long Id);
    }
}
