﻿using System.ComponentModel.DataAnnotations.Schema;

namespace app.EntityModel.CoreModel
{
    [Table("VouchersLines", Schema = "New")]
    public sealed class VouchersLines : BaseEntity
    {
        public long VouchersId { get; set; }
        public Vouchers Vouchers { get; set; }
        [ForeignKey(nameof(ChartOfAccounts))]
        public string AccountCode { get; set; }
        public ChartOfAccounts ChartOfAccounts { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Particular { get; set; }
        public string AttachmentPath { get; set; }
    }
}
