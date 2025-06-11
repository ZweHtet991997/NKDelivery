using DeliverySystemAPI.Services.Accounting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Services.OverAll
{
    public class AccountingDebitOverall
    {
        public Object Get_Accounting_Debit_Overall(String Business_Name, SqlConnection con)
        {
            DataTable dt = new DataTable();
            return JsonConvert.SerializeObject(dt);

        }
        public Object Get_Accounting_To_Pay(String Client_Name, String Payment_Status, String From_Date, String To_Date, String Business_Name, SqlConnection con)
        {
            /*            con.Open();*/
            SqlCommand get_cmd = null;
            List<int> co_id_list = new List<int>();
            string[] client_name = new string[100000], collect_items = new string[100000], assign_date = new string[100000], payment_date = new string[100000];
            List<int> total_amount = new List<int>();
            //var client_name = ""; var collect_items = ""; var assign_date = ""; var payment_date = "";
            if (From_Date == null && To_Date == null)
            {
                //get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY CO_ID ASC) AS SrNo,CO_ID,Client_Name,Total_Amount ,Gate_Amount,All_Paid_Amount,Create_Date,Payment_Date From ClientOrder Where Client_Name Like @client_name And Payment_Status=@payment_status And Client_Pay_Type=@client_pay_type And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY ClientOrder.Create_Date DESC) AS SrNo,Count(CollectAssignOrder.CO_ID) As Collect_Items , ClientOrder.CO_ID,Client_Name,Gate_Amount,All_Paid_Amount,Sum(CollectAssignOrder.Item_Price) As Total_Amount,To_Pay_Paid_Amount,ClientOrder.Create_Date,ClientOrder.Payment_Date,Order_Check_Status From ClientOrder Inner Join CollectAssignOrder On (ClientOrder.CO_ID=CollectAssignOrder.CO_ID) Where Client_Name Like @client_name And ClientOrder.Payment_Status=@payment_status And Client_Pay_Type=@client_pay_type And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted Group By ClientOrder.CO_ID,ClientOrder.Client_Name,Gate_Amount,All_Paid_Amount,To_Pay_Paid_Amount,ClientOrder.Create_Date,ClientOrder.Payment_Date,Order_Check_Status", con);
                get_cmd.Parameters.AddWithValue("@client_pay_type", "Debit");
            }
            else
            {
                get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY ClientOrder.Create_Date DESC) AS SrNo,Count(CollectAssignOrder.CO_ID) As Collect_Items , ClientOrder.CO_ID,Client_Name,Gate_Amount,All_Paid_Amount,Sum(CollectAssignOrder.Item_Price) As Total_Amount,To_Pay_Paid_Amount,ClientOrder.Create_Date,ClientOrder.Payment_Date,Order_Check_Status From ClientOrder Inner Join CollectAssignOrder On (ClientOrder.CO_ID=CollectAssignOrder.CO_ID) Where Client_Name Like @client_name And ClientOrder.Payment_Status=@payment_status And Client_Pay_Type=@client_pay_type And cast(ClientOrder.[Payment_Date] as date) between @from_date and @to_date And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted Group By ClientOrder.CO_ID,ClientOrder.Client_Name,Gate_Amount,All_Paid_Amount,To_Pay_Paid_Amount,ClientOrder.Create_Date,ClientOrder.Payment_Date,Order_Check_Status", con);
                get_cmd.Parameters.AddWithValue("@client_pay_type", "Debit");
                get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                get_cmd.Parameters.AddWithValue("@to_date", To_Date);
            }
            get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
            get_cmd.Parameters.AddWithValue("@payment_status", Payment_Status);
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            SqlDataReader reader = get_cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("SrNo");
            dt.Columns.Add("CO_ID");
            dt.Columns.Add("Client_Name");
            dt.Columns.Add("Total_Amount");
            dt.Columns.Add("Gate_Amount");
            dt.Columns.Add("All_Paid_Amount");
            dt.Columns.Add("To_Pay_Paid_Amount");
            dt.Columns.Add("Net_Amount");
            dt.Columns.Add("Collect_Items");
            dt.Columns.Add("Create_Date");
            dt.Columns.Add("Payment_Date");
            dt.Columns.Add("Order_Check_Status");
            List<int> gate_amount_list = new List<int>();
            List<int> all_paid_amount_list = new List<int>();
            List<int> to_pay_paid_amount_list = new List<int>();
            List<string> order_check_status_list = new List<string>();
            //var sr_no = "";
            int i = 0;
            while (reader.Read())
            {
                //sr_no = reader["SrNo"].ToString();
                co_id_list.Add(Convert.ToInt32(reader["CO_ID"].ToString()));
                client_name[i] = reader["client_Name"].ToString();
                collect_items[i] = reader["Collect_Items"].ToString();
                total_amount.Add(Convert.ToInt32(reader["Total_Amount"]));
                assign_date[i] = reader["Create_Date"].ToString();
                payment_date[i] = reader["Payment_Date"].ToString();
                i++;
                if (reader["Gate_Amount"].ToString() == "" || reader["Gate_Amount"].ToString() == null)
                {
                    gate_amount_list.Add(0);
                }
                else
                {
                    gate_amount_list.Add(Convert.ToInt32(reader["Gate_Amount"].ToString()));
                }
                if (reader["All_Paid_Amount"].ToString() == "" || reader["All_Paid_Amount"].ToString() == null)
                {
                    all_paid_amount_list.Add(0);
                }
                else
                {
                    all_paid_amount_list.Add(Convert.ToInt32(reader["All_Paid_Amount"].ToString()));
                }
                //var net_amount = Convert.ToInt32(total_amount) - (Convert.ToInt32(gate_amount) + Convert.ToInt32(all_paid_amount));
                if (reader["To_Pay_Paid_Amount"].ToString() == "" || reader["To_Pay_Paid_Amount"].ToString() == null)
                {
                    to_pay_paid_amount_list.Add(0);
                }
                else
                {
                    to_pay_paid_amount_list.Add(Convert.ToInt32(reader["To_Pay_Paid_Amount"].ToString()));
                }
                order_check_status_list.Add(reader["Order_Check_Status"].ToString());
                //Get_Collect_Total_Amount_By_CO_ID call_service = new Get_Collect_Total_Amount_By_CO_ID();
                //var total_amount = call_service.GetCollectTotalAmountByCOID(co_id, Business_Name, con);
                //var net_amount = total_amount - (gate_amount + all_paid_amount)-to_pay_paid_amount;
                //dt.Rows.Add(sr_no, co_id, client_name, total_amount, gate_amount, all_paid_amount,to_pay_paid_amount, net_amount,collect_items, assign_date, payment_date);
            }
            reader.Close();
            int j = 0;
            foreach (var co_id in co_id_list)
            {
                Get_Collect_Total_Amount_By_CO_ID call_service = new Get_Collect_Total_Amount_By_CO_ID();
                //total_amount = call_service.GetCollectTotalAmountByCOID(co_id, Business_Name, con);
                var gate_amount = gate_amount_list[j];
                var all_paid_amount = all_paid_amount_list[j];
                var to_pay_paid_amount = to_pay_paid_amount_list[j];
                var net_amount = total_amount[j] - (gate_amount + all_paid_amount - to_pay_paid_amount);
                var order_check_status = order_check_status_list[j];
                dt.Rows.Add(j + 1, co_id, client_name[j].ToString(), total_amount[j], gate_amount, all_paid_amount, to_pay_paid_amount, net_amount, collect_items[j].ToString(), assign_date[j].ToString(), payment_date[j].ToString(), order_check_status);
                j++;
            }

            //SqlDataAdapter da = new SqlDataAdapter(get_cmd);
            //da.Fill(dt);
            GC.Collect();
            return JsonConvert.SerializeObject(dt);
        }
        public Object Get_Return_Item_Data(String Client_Name, String Payment_Status, String From_Date, String To_Date, String Business_Name, SqlConnection con)
        {
            SqlCommand get_cmd = null;
            DataTable dt = new DataTable();
            if (From_Date == null && To_Date == null)
            {
                get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY Complete_Date DESC) As SrNo,ClientOrder.Client_Name,CollectAssignOrder.Business_Name,CollectAssignOrder.IsDeleted,CollectAssignOrder.CO_ID,CAO_ID,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Deli_Price,Expense_Fees,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,CollectAssignOrder.Payment_Date,Complete_Date From CollectAssignOrder Inner Join  ClientOrder On(ClientOrder.CO_ID=CollectAssignOrder.CO_ID) And CollectAssignOrder.CO_ID in (Select CO_ID From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted)  And (Deli_Status=@deli_status1 OR Deli_Status=@deli_status2)  And Deli_Type=@deli_type  And CollectAssignOrder.Payment_Status=@payment_status", con);
            }
            else
            {
                get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY Complete_Date DESC) As SrNo,ClientOrder.Client_Name,CollectAssignOrder.Business_Name,CollectAssignOrder.IsDeleted,CollectAssignOrder.CO_ID,CAO_ID,Receiver_Name,Deli_Men,C_Name,T_Name,Tan_Sar_Price,CollectAssignOrder.Total_Amount,Item_Price,Deli_Price,Expense_Fees,Return_Item_Amount,Deli_Type,CollectAssignOrder.Create_Date,CollectAssignOrder.Payment_Date,Complete_Date From CollectAssignOrder Inner Join  ClientOrder On(ClientOrder.CO_ID=CollectAssignOrder.CO_ID) And CollectAssignOrder.CO_ID in (Select CO_ID From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted)  And (Deli_Status=@deli_status1 OR Deli_Status=@deli_status2) And Deli_Type=@deli_type  And CollectAssignOrder.Payment_Status=@payment_status And cast(CollectAssignOrder.[Payment_Date] as date) between @from_date And @to_date", con);
                get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                get_cmd.Parameters.AddWithValue("@to_date", To_Date);
            }
            get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
            get_cmd.Parameters.AddWithValue("@deli_status1", "Completed");
            get_cmd.Parameters.AddWithValue("@deli_status2", "Accepted");
            get_cmd.Parameters.AddWithValue("@deli_type", "Return Item");
            get_cmd.Parameters.AddWithValue("@payment_status", Payment_Status);
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            SqlDataAdapter da = new SqlDataAdapter(get_cmd);
            da.Fill(dt);
            GC.Collect();
            return JsonConvert.SerializeObject(dt);
        }
    }
}
