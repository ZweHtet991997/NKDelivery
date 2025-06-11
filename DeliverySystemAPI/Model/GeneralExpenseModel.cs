using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class GeneralExpenseModel:DefaultModel
    {
        public int GEXP_ID { get; set; }
        public string General_Expense_Name { get; set; }
        public string General_Expense_Detail { get; set; }
        public int General_Expense_Amount { get; set; }
        public string Create_Date { get; set; }
    }
}
