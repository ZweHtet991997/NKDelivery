using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class BalanceSheetModel : DefaultModel
    {
        public int BalanceBalanceSheet_ID { get; set; }
        public int Cashin { get; set; }
        public int Kpay { get; set; }
        public string DeliMen { get; set; }
        public int Total_Amount { get; set; }
        public int ToGet { get; set; }
        public int ToPay { get; set; }
        public int GeneralExpense { get; set; }
        public string Remark { get; set; }
        public string CreateDate { get; set; }

    }
}
