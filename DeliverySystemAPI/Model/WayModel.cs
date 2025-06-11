using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class WayModel:DefaultModel
    {
        public int CAO_ID { get; set; }
        public String Deli_Men { get; set; }
        public String Deli_Status { get; set; }
        public String Receiver_Name { get; set; }
        public String C_Name { get; set; }
        public String T_Name { get; set; }
        public int Tan_Sar_Price { get; set; }
        public String G_Name { get; set; }
        public int G_Price { get; set; }
        public int Item_Price { get; set; }
        public int Expense_Fees { get; set; }
        public int Half_Paid_Amount { get; set; }
        public int Return_Item_Amount { get; set; }
        public String Deli_Type { get; set; }
        public int Total_Amount { get; set; }
        public String Expense_Status { get; set; }
        public String Deli_Men_Remark { get; set; }
        public String Assign_Remark { get; set; }
    }
}
