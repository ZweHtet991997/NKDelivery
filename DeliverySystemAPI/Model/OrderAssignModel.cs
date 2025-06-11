using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class OrderAssignModel:DefaultModel
    {
        public String User_Name { get; set; }
        public int Delivery_Fees { get; set; }
        public int Expense_Fees { get; set; }
        public String Receiver_Name { get; set; }
        public String Receiver_Address { get; set; }
        public String Receiver_Phone_Number { get; set; }
        public String Receiver_Payment { get; set; }
        public int Receiver_Payment_Amount { get; set; }
        public String Delivery_Assign_Date { get; set; }
        public int Order_ID { get; set; }

    }
}
