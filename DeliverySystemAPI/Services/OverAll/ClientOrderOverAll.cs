
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Services.OverAll
{
    public class ClientOrderOverAll
    {
        public Object Count_CollectAssign_By_COID(String CO_ID,String Business_Name,SqlConnection con)
        {
            con.Open();
            SqlCommand get_cmd = new SqlCommand("Select COUNT(CAO_ID) As Total_Number_Of_Collect_Assign From CollectAssignOrder Where CO_ID=@co_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@co_id", CO_ID);
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            SqlDataAdapter da = new SqlDataAdapter(get_cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            GC.Collect();
            return JsonConvert.SerializeObject(dt);
        }
    }
}
