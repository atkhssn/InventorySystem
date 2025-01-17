using System.ComponentModel.DataAnnotations;

namespace app.Utility
{
    public enum ProductType
    {
        Raw_Product = 'R',
        Finish_Product = 'F',
    }

    public enum Unit
    {
        KG,
        Pcs,
        Packet,
        Gm,
    }
    public enum VendorType
    {
        Supplier = 1,
        Customer = 2,
    }
    public enum PaymentType
    {
        Credit = 1,
        Cash = 2,
        Special = 3,
    }
    public enum PaymentMethod
    {
        Cash_Hand = 1,
        Bank = 2,
        Mobile_Banking = 3,
    }
    public enum CustomerType
    {
        Dealer = 1,
        Retail = 2,
        Corporate = 3,
        Individual = 4,
    }
    public enum StockType
    {
        PV = 1,
        SV = 2,
        PRV = 3,
        SRV = 4,
    }
    public enum OtherExpensesType
    {
        Rent = 1,
        Repairs = 2,
        Insurance = 3,
        Rates_and_Taxes = 4,
        Tax_Penalties = 5,
        Conveyance = 6,
        Consumptions_of_Spares = 7,
        Salary = 8,
        Refresment = 9,
        Utility_Bill = 10,
        Others = 20,
    }

    public enum VoucherStatus
    {
        CREATED = 1,
        UPDATED = 2,
        DELETED = 3,
        SUBMITTED = 4,
        APPROVED = 5,
        REJECTED = 6,
    }

    public enum CoATypes
    {
        ASSET = 1,
        LIABILITY = 2,
        EQUITY = 3,
        REVENUE = 4,
        EXPENSE = 5,
    }

    public enum CoAHead
    {
        HEADONE = 1,
        HEADTWO = 2,
        HEADTHREE = 3,
        HEADFOUR = 4,
        HEADFIVE = 5,
    }

    public enum NewVoucherTypes
    {
        [Display(Name = "Bank Payment")]
        BankPayment = 13,
        [Display(Name = "Bank Receive")]
        BankReceive = 14,
        [Display(Name = "Cash Payment")]
        CashPayment = 15,
        [Display(Name = "Cash Receive")]
        CashReceive = 16,
        [Display(Name = "Contra Voucher")]
        ContraVoucher = 17,
        [Display(Name = "Journal Voucher")]
        JournalVoucher = 18,
    }
}
