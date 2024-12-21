using app.Utility.DtoModel;

namespace app.Services.Accounting
{
    public interface IAccountingService
    {
        Task<CostCentersViewModel> CostCentersAsync();
        Task<CostCentersViewModel> CostCenterAync(long id);
        Task<ResponseViewModel> AddCostCenterAsync(CostCentersViewModel model);
        Task<ResponseViewModel> UpdateCostCenterAync(CostCentersViewModel model);
        Task<ResponseViewModel> DeleteCostCenterAync(long id);

        Task<ChartOfAccountsViewModel> ChartOfAccoutingsAsync();
        Task<ResponseViewModel> AddAccountHeadAsync(ChartOfAccountsViewModel model);
        Task<ResponseViewModel> UpdateAccountHeadAsync(ChartOfAccountsViewModel model);
        Task<ResponseViewModel> DeleteAccountHeadAsync(string accountCode);

        Task<List<ChartOfAccountHierarchy>> GetGLAccountHeadAsync();
        Task<ResponseViewModel> BulkUploadAccountHead(List<ChartOfAccoutDtoModel> modelList);

        Task<ChartOfAccountsViewModel> ReceivableAccountHeadsAsync();
        Task<ChartOfAccountsViewModel> PayableAccountHeadsAsync();
        Task<ChartOfAccountsViewModel> CashBookAccountHeadsAsync();
        Task<ChartOfAccountsViewModel> BankBookAccountHeadsAsync();
    }
}
