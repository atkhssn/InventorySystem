using System.ComponentModel.DataAnnotations;

namespace app.Services.Vouchern
{
    public sealed class VouchersLinesViewModel : BaseViewModel
    {
        [Display(Name = "Voucher")]
        public long VouchersId { get; set; }

        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }

        public VouchersViewModel VouchersViewModel { get; set; }

        [Display(Name = "GlHead")]
        public long GlHeadId { get; set; }

        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; }

        [Display(Name = "Cedit Amount")]
        public decimal CreditAmount { get; set; }

        [Display(Name = "Particular")]
        public string Particular { get; set; }

        public List<VouchersLinesViewModel> VouchersLinesViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
