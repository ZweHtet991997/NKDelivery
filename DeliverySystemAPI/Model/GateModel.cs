using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class GateModel:DefaultModel
    {
        public int G_ID { get; set; }
        public String Deli_Men { get; set; }
        public String G_Name { get; set; }
        public int G_Price { get; set; }
        public int Expense_Fees { get; set; }
    }
}
