using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class OrderModel:DefaultModel
    {
        //Order Create
        public String Client_Name { get; set; }
        public String Deli_Assign_Date { get; set; }
        public String Deli_Township { get; set; }
        public String Client_Payment { get; set; }
        public int Order_Quantity { get; set; }
        public int Order_Amount { get; set; }
        public int Deli_Fees { get; set; }
        public int Expense_Fees { get; set; }
        public String Receiver_Payment { get; set; }
        public int Total_Amount { get; set; }
        public String User_Name { get; set; }
        public String Order_Remark { get; set; }
    }
}
