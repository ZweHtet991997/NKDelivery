using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Services.Accounting
{
    public class Get_Collect_Total_Amount_By_CO_ID
    {
        public int GetCollectTotalAmountByCOID(int CO_ID,String Business_Name,SqlConnection con)
        {
            SqlCommand get_collect_total = new SqlCommand("Select SUM(Item_Price)As Total_Amount From CollectAssignOrder Where CO_ID=@co_id And Payment_Status=@payment_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_collect_total.Parameters.AddWithValue("@co_id", CO_ID);
            get_collect_total.Parameters.AddWithValue("@payment_status", "Pending");
            get_collect_total.Parameters.AddWithValue("@business_name", Business_Name);
            get_collect_total.Parameters.AddWithValue("@isdeleted", '0');
            SqlDataReader reader = get_collect_total.ExecuteReader();
            int total_amount = 0;
            while (reader.Read())
            {
                if (reader["Total_Amount"].ToString() == "" || reader["Total_Amount"].ToString() == null)
                {
                    total_amount = 0;
                }
                else
                {
                    total_amount = Convert.ToInt32(reader["Total_Amount"].ToString());
                }
            }
            reader.Close();
            return total_amount;
        }
    }
}
