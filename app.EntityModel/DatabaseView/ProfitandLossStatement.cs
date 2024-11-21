using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.EntityModel.DatabaseView
{
    public class ProfitandLossStatement
    {
        [Key]
        public int Serialno {get; set; }
        public string UsersName {get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal Purches { get; set; }
        public decimal SupplierPayment { get; set; }
        public decimal OtherExpenses { get; set; }
        public decimal BankCharg { get; set; }
        public decimal TransportCharges { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal Sales { get; set; }
        public decimal CustomerCollaction { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal SalesReturn { get; set; }
        public decimal PurchesReturn { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal ClosingStock { get; set; }
    }
}
