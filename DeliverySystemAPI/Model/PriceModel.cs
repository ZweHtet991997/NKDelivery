using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class PriceModel:DefaultModel
    {
        public int P_ID { get; set; }
        public int Deli_Price { get; set; }
        public int Expense_Fees { get; set; }
        public String C_Name { get; set; }
        public String T_Name { get; set; }
    }
}
