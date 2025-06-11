using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class CollectAssignOrderModel:DefaultModel
    {
        public int CAO_ID { get; set; }
        public int CO_ID { get; set; }
        public String Receiver_Name { get; set; }
        public String Assign_Date { get; set; }
        public String C_Name { get; set; }
        public String T_Name { get; set; }
        public String G_Name { get; set; }
        public int G_Price { get; set; }
        public int Item_Price { get; set; }
        public int Deli_Price { get; set; }
        public int Expense_Fees { get; set; }
        public String Deli_Type { get; set; }
        public String  Deli_Men { get; set; }
        public int Total_Amount { get; set; }
        public String Payment_Status { get; set; }
        public String Expense_Status { get; set; }
        public String Assign_Remark { get; set; }
    }
}
