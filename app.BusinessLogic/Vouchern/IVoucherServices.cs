namespace app.Services.Vouchern
{
    public interface IVoucherServices
    {
        Task<VoucherTypesViewModel> VoucherTypesAsync();
        Task<VoucherTypesViewModel> VoucherTypeAsync(long Id);
        Task<ResponseViewModel> AddVoucherTypeAsync(VoucherTypesViewModel model);
        Task<ResponseViewModel> UpdateVoucherTypeAync(VoucherTypesViewModel model);
        Task<ResponseViewModel> DeleteVoucherTypeAync(long Id);

        Task<VouchersViewModel> VouchersAsync();
        Task<VouchersViewModel> VoucherAsync(long Id);
        Task<VouchersViewModel> VoucherByVoucherNoAsync(string voucherNo);
        Task<VouchersViewModel> AddVoucherAsync(VouchersViewModel model);
        Task<ResponseViewModel> DeleteVoucherAync(long Id);

        Task<VouchersLinesViewModel> VoucherLinesAsync();
        Task<VouchersLinesViewModel> VoucherLineAsync(long Id);
        Task<VouchersLinesViewModel> VoucherLinesByVoucherIdAsync(long Id);
        Task<VouchersViewModel> AddVoucherLineAsync(VouchersViewModel model);
        Task<ResponseViewModel> SubmitVoucherAync(long Id);
    }
}
