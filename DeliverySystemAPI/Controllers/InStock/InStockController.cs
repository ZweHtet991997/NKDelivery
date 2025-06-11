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

namespace DeliverySystemAPI.Controllers.InStock
{
    public class InStockController : Controller
    {
        private IConfiguration Configuration;
        public InStockController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/instock")]
        public IActionResult Index(String Deli_Men,String Deli_Status,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY Instock_Date DESC) AS SrNo,ClientOrder.Client_Name,CAO_ID,Deli_Men,Receiver_Name,C_Name,T_Name,Deli_Type,Item_Price,G_Name,G_Price,Tan_Sar_Price,Expense_Fees,Return_Item_Amount,Half_Paid_Amount,CollectAssignOrder.Total_Amount,Deli_Status,Deli_Men_Remark,Instock_Date,CollectAssignOrder.Create_Date From CollectAssignOrder Left Join ClientOrder On CollectAssignOrder.CO_ID=ClientOrder.CO_ID Where CollectAssignOrder.Deli_Men Like @deli_men And CollectAssignOrder.Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                get_cmd.Parameters.AddWithValue("@deli_status",Deli_Status);
/*                get_cmd.Parameters.AddWithValue("@payment_status","Pending");*/
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK,JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
            }
        #endregion Get
        #region Update
        [HttpPut]
        [Route("api/instock")]
        public IActionResult Update_Deli_Status(int CAO_ID,String Deli_Men,String Deli_Status,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = null;
                if (Deli_Status == "Assigned")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    update_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                    //update_cmd.Parameters.AddWithValue("@assign_date", DateTime.Now);
                }
                else
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder  Set Deli_Status=@deli_status,Rejected_Status=@rejected_status,Rejected_Pending_Date=@rejected_pending_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    update_cmd.Parameters.AddWithValue("@rejected_status", "Pending");
                    update_cmd.Parameters.AddWithValue("@rejected_pending_date", DateTime.Now);
                }                
                update_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                update_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Updated Deli Status");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update

    }
}
