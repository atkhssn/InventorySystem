namespace app.Services.Vouchern
{
    public interface IVoucherServices
    {
        Task<VoucherTypesViewModel> VoucherTypesAsync();
        Task<VoucherTypesViewModel> VoucherTypeAsync(long id);
        Task<ResponseViewModel> AddVoucherTypeAsync(VoucherTypesViewModel model);
        Task<ResponseViewModel> UpdateVoucherTypeAync(VoucherTypesViewModel model);
        Task<ResponseViewModel> DeleteVoucherTypeAync(long id);
        Task<VouchersViewModel> VouchersAsync();
        Task<VouchersViewModel> VoucherAsync(long Id);
        Task<VouchersViewModel> UnapprovedVoucherList();
        Task<VouchersViewModel> VoucherByVoucherNoAsync(string voucherNo);
        Task<VouchersViewModel> AddVoucherAsync(VouchersViewModel model);
        Task<ResponseViewModel> DeleteVoucherAync(long Id);
        Task<ResponseViewModel> DeleteVoucherLineAync(long Id);
        Task<VouchersViewModel> AddVoucherLineAsync(VouchersViewModel model);
        Task<ResponseViewModel> MakeSubmitVoucherAync(long Id);
        Task<ResponseViewModel> MakeApproveVoucherAsync(string voucherNo);
        Task<SearchVoucherViewModel> SearchVoucherAsync(SearchVoucherViewModel model);
    }
}
