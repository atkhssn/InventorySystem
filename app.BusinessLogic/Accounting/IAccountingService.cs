namespace app.Services.Accounting
{
    public interface IAccountingService
    {
        Task<CostCenterViewModel> GetAllRecordAsync();
        Task<CostCenterViewModel> GetRecordDetailAync(long Id);
        Task<ResponseViewModel> AddRecordAsync(CostCenterViewModel model);
        Task<ResponseViewModel> UpdateRecordAync(CostCenterViewModel model);
        Task<ResponseViewModel> DeleteRecordAync(long Id);
    }
}
