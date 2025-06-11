using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Model
{
    public class AccountModel:DefaultModel
    {
        public int User_ID { get; set; }
        public String User_Name { get; set; }
        public String Phone_Number { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String User_Role { get; set; }
        public String Client_Name { get; set; }
        public String City { get; set; }
        public String[] Township { get; set; }
        public String Address { get; set; }

    }
}
