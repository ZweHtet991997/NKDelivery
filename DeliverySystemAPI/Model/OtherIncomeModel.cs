using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class OtherIncomeModel:DefaultModel
    {
        public int OI_ID { get; set; }
        public string Other_Income_Name { get; set; }
        public int Other_Income_Amount { get; set; }
        public string Other_Income_Detail { get; set; }
    }
}
