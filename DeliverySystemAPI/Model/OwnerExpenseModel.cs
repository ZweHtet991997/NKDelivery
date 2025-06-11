using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class OwnerExpenseModel:DefaultModel
    {
        public int OEXP_ID { get; set; }
        public string Owner_Expense_Detail { get; set; }
        public int Owner_Expense_Amount { get; set; }
    }
}
