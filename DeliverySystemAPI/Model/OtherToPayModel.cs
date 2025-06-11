using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class OtherToPayModel:DefaultModel
    {
        public int OI_ID { get; set; }
        public string Other_ToPay_Name { get; set; }
        public int Other_ToPay_Amount { get; set; }
        public string Other_ToPay_Detail { get; set; }
    }
}
