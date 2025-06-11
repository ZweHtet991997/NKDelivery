using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class DefaultModel
    {
        public String Business_Name { get; set; }
        public char IsDeleted { get; set; }
        public String DB_Name = "PostExpressMyanmarDBConnection";
          //public String DB_Name = "NPT_SHWE_DeliveryDBConnection";
    }
}
