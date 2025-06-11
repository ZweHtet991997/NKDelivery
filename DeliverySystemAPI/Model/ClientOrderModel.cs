using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class ClientOrderModel:DefaultModel
    {
        public int CO_ID { get; set; }
        public string Client_Name { get; set; }
        public int Total_Amount { get; set; }
        public int Gate_Amount { get; set; }
        public int All_Paid_Amount { get; set; }
        public string Client_Pay_Type { get; set; }
        public string Order_Check_Status { get; set; }
        public string Payment_Status { get; set; }
        public int Item_Quantity { get; set; }
        public String Pickup_Date { get; set; }
        public string Remark { get; set; }
    }
}
