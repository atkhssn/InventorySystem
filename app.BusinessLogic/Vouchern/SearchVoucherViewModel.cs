using app.Services.Accounting;
using System.ComponentModel.DataAnnotations;

namespace app.Services.Vouchern
{
    public sealed class SearchVoucherViewModel
    {
        [Display(Name = "Voucher Type")]
        public long? VoucherTypesId { get; set; }
        public VoucherTypesViewModel VoucherTypesViewModel { get; set; }

        [Display(Name = "Cost Center")]
        public long? CostCentersId { get; set; }
        public CostCentersViewModel CostCenterViewModel { get; set; }

        [Display(Name = "From Date")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "To Date")]
        public DateTime? ToDate { get; set; }

        public List<VouchersViewModel> VouchersViewModels { get; set; }
    }
}
