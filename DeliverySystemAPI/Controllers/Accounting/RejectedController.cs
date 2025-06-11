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
    public class RejectedController : Controller
    {
        private IConfiguration Configuration;
        public RejectedController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/accounting/rejected")]
        public IActionResult Get_Rejected_List(String Client_Name,String Rejected_Status,String From_Date,String To_Date,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = null;
                if (Rejected_Status == "Pending")
                {
                    //get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY Instock_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Instock_Date,CollectAssignOrder.Create_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where CollectAssignOrder.Deli_Men Like @deli_men And Rejected_Status=@rejected_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                    get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY Instock_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Instock_Date,CollectAssignOrder.Create_Date From CollectAssignOrder Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where ClientOrder.Client_Name Like @client_name And Rejected_Status=@rejected_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                }
                //Rejected Status='Completed'
                else
                {
                    if(From_Date==null && To_Date == null)
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY Instock_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Instock_Date,Rejected_Complete_Date,CollectAssignOrder.Create_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where ClientOrder.Client_Name Like @client_name And CollectAssignOrder.Deli_Status='Rejected'  And Rejected_Status=@rejected_status  And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                    }
                    else
                    {
                        get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY Instock_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Instock_Date,Rejected_Complete_Date,CollectAssignOrder.Create_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where ClientOrder.Client_Name Like @client_name And CollectAssignOrder.Deli_Status='Rejected'  And Rejected_Status=@rejected_status  And cast([Rejected_Complete_Date]as date)between @from_date And @to_date And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                        get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                        get_cmd.Parameters.AddWithValue("@to_date", To_Date);
                    }
                    
                }                
                get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
                get_cmd.Parameters.AddWithValue("@rejected_status", Rejected_Status);
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                get_cmd.Parameters.AddWithValue("@deli_type", "Return Item");
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        [HttpPut]
        [Route("api/accounting/rejected")]
        public IActionResult Update_Rejected_List(String CAO_ID, String Rejected_Status, String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Rejected_Status=@rejected_status,Rejected_Complete_Date=@rejected_complete_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted",con);
                update_cmd.Parameters.AddWithValue("@rejected_status", Rejected_Status);
                update_cmd.Parameters.AddWithValue("@rejected_complete_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, "Update Reject Status");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        [HttpGet]
        [Route("api/accounting/rejected/client")]
        public IActionResult Get_Rejected_List_By_Client(String Client, String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select SUM(Item_Price) As Rejected_Amount From CollectAssignOrder Where CollectAssignOrder.CO_ID in (Select ClientOrder.CO_ID From ClientOrder Where Client_Name Like @client_name And Business_Name=@business_name And IsDeleted=@isdeleted)  And Rejected_Status=@rejected_status And cast([Rejected_Pending_Date] as date) between @from_date And @to_date And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@client_name", Client);
                get_cmd.Parameters.AddWithValue("@rejected_status", "Pending");
                get_cmd.Parameters.AddWithValue("@from_date", DateTime.Now.ToString("yyyy-MM-dd"));
                get_cmd.Parameters.AddWithValue("@to_date", DateTime.Now.ToString("yyyy-MM-dd"));
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }       
        [HttpPut]
        [Route("api/accounting/rejected/client")]
        public IActionResult Update_Rejected_List_By_Client(String CAO_ID, String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Rejected_Status=@rejected_status,Set Rejected_Complete_Date=@rejected_complete_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@rejected_status", "Completed");
                update_cmd.Parameters.AddWithValue("@rejected_complete_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK,"Reject Completed");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
