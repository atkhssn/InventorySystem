using app.Services.Accounting;
using app.Utility;
using System.ComponentModel.DataAnnotations;

namespace app.Services.Vouchern
{
    public sealed class VouchersViewModel : BaseViewModel
    {
        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }

        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Voucher Type")]
        public long VoucherTypesId { get; set; }
        public VoucherTypesViewModel VoucherTypesViewModel { get; set; }

        [Display(Name = "Cost Center")]
        public long CostCentersId { get; set; }
        public CostCentersViewModel CostCenterViewModel { get; set; }

        [Display(Name = "Total Debit Amount")]
        public decimal TotalDebitAmount { get; set; }

        [Display(Name = "Total Credit Amount")]
        public decimal TotalCreditAmount { get; set; }

        [Display(Name = "Narration")]
        public string Narration { get; set; }

        [Display(Name = "Status")]
        public VoucherStatus VoucherStatus { get; set; }

        public VouchersLinesViewModel VouchersLinesViewModel { get; set; }
        public List<VouchersViewModel> VouchersViewModels { get; set; }
        public List<VouchersLinesViewModel> VouchersLinesViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
