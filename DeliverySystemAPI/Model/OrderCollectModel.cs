using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class OrderCollectModel:DefaultModel
    {
        public int Collect_Quantity { get; set; }
        public int Collect_Amount { get; set; }
        public String Collect_Remark { get; set; }
        public String Order_Status { get; set; }
        public int Amount_Difference { get; set; }
        public int Client_Paid_Amount { get; set; }
        public String Client_Payment_Status { get; set; }
        public int Order_ID { get; set; }
    }
}
