using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class ClientModel:DefaultModel
    {
        public int Client_ID { get; set; }
        public String Client_Name { get; set; }
        public String Phone_Number { get; set; }
        public String Address { get; set; }
        public String Bank_Account { get; set; }
        public String Bank_Account_Owner { get; set; }
        public String  Contact_Person { get; set; }
    }
}
