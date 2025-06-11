using DeliverySystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Controllers.Accounting
{
    public class ProfitController : ControllerBase
    {
        private IConfiguration Configuration;
        public ProfitController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/accounting/profit")]
        public IActionResult Index(String From_Date,String To_Date,String Business_Name)
        {
            if(From_Date==null && To_Date == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Please select Date");
            }
            else
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                DataTable dt = new DataTable();
                dt.Columns.Add("SrNo");
                dt.Columns.Add("Deli_Men");
                dt.Columns.Add("Total_Deli",typeof(int));
                dt.Columns.Add("Deli_Expense", typeof(int));
                dt.Columns.Add("Net_Profit", typeof(int));
                dt.Columns.Add("Ways", typeof(int));
                dt.Columns.Add("General_Expense_Amount", typeof(int));
                //Getting Deli Men , Deli Price , Expense Fees
                SqlCommand get_cmd1 = new SqlCommand("Select  Deli_Men,SUM(Deli_Price) As Total_Deli,SUM(G_Price) As Gate_Price,SUM(Expense_Fees) As Deli_Expense , Count(Deli_Men) As Ways From CollectAssignOrder Where cast([Accept_Date] as date) between @from_date and @to_date And Deli_Status=@deli_status And Business_Name=@business_name And IsDeleted=@isdeleted Group By Deli_Men", con);
                get_cmd1.Parameters.AddWithValue("@from_date", From_Date);
                get_cmd1.Parameters.AddWithValue("@to_date", To_Date);
                get_cmd1.Parameters.AddWithValue("@deli_status", "Accepted");
                get_cmd1.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd1.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataReader reader1 = get_cmd1.ExecuteReader();
                var sr_no = 0;
                while (reader1.Read())
                {
                    sr_no++;
                    string deli_men = reader1["Deli_Men"].ToString();
                    int deli_price = Convert.ToInt32(reader1["Total_Deli"].ToString());
                    int gate_price = Convert.ToInt32(reader1["Gate_Price"].ToString());
                    int expense_fees = Convert.ToInt32(reader1["Deli_Expense"].ToString());
                    //int net_profit =(deli_price+gate_price)-expense_fees;
                    //update taung par tae Ma Min See ka nay 04 May 2022
                    int net_profit = deli_price - expense_fees;          
                    int ways = Convert.ToInt32(reader1["Ways"].ToString());
                    dt.Rows.Add( sr_no,deli_men, deli_price, expense_fees,net_profit,ways, null);
                }
                reader1.Close();
                //Getting Total General Expense Amount
               /* SqlCommand get_cmd2 = new SqlCommand("Select SUM(General_Expense_Amount) As General_Expense_Amount From GeneralExpense Where cast([Create_Date]as date) between @from_date and @to_date And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_cmd2.Parameters.AddWithValue("@from_date", From_Date);
                get_cmd2.Parameters.AddWithValue("@to_date", To_Date);
                get_cmd2.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd2.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataReader reader2 = get_cmd2.ExecuteReader();
                int general_expense_amount = 0;
                while (reader2.Read())
                {
                    string test = reader2["General_Expense_Amount"].ToString();
                    if (reader2["General_Expense_Amount"].ToString()!=null && reader2["General_Expense_Amount"].ToString() != "")
                    {
                        general_expense_amount = Convert.ToInt32(reader2["General_Expense_Amount"].ToString());
                        dt.Rows.Add(null, null, null, null, general_expense_amount);
                    }
                    
                }
                reader2.Close();*/
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
        }
    }
}
