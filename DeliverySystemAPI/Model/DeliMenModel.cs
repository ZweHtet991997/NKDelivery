using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class DeliMenModel:DefaultModel
    {
        public int CAO_ID { get; set; }
        public String Deli_Status { get; set; }
        public int Tan_Sar_Price { get; set; }
        public String Deli_Men_Remark { get; set; }
        public String Deli_Type { get; set; }
        public int Return_Or_Half_Amount { get; set; }
    }
}
