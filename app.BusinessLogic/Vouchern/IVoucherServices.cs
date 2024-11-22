namespace app.Services.Vouchern
{
    public interface IVoucherServices
    {
        Task<VoucherTypesViewModel> VoucherTypesAsync();
        Task<VoucherTypesViewModel> VoucherTypeAsync(long Id);
        Task<ResponseViewModel> AddVoucherTypeAsync(VoucherTypesViewModel model);
        Task<ResponseViewModel> UpdateVoucherTypeAync(VoucherTypesViewModel model);
        Task<ResponseViewModel> DeleteVoucherTypeAync(long Id);
    }
}
