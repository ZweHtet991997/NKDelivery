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
namespace DeliverySystemAPI.Controllers.Order
{
    public class CollectAssignOrderController : ControllerBase
    {
        private IConfiguration Configuration;
        public CollectAssignOrderController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/order/collectassign")]
        public IActionResult Get_Collect_Assign_Order(int CO_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CAO_ID ASC) AS SrNo,CAO_ID,CO_ID,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Item_Price,Deli_Price,Expense_Fees,Deli_Type,Deli_Men,Total_Amount,Payment_Status,Deli_Status,Expense_Status,Assign_Remark,Create_Date,Accept_Date From CollectAssignOrder Where CO_ID=@co_id  And  Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@co_id", CO_ID);
/*                get_cmd.Parameters.AddWithValue("@deli_status", "Accepted");*/
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get
        #region Create
        [HttpPost]
        [Route("api/order/collectassign")]
        public IActionResult Create_Collect_Assign_Order([FromBody]CollectAssignOrderModel collectAssignOrderModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(collectAssignOrderModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand create_cmd = new SqlCommand("Insert Into CollectAssignOrder(CO_ID,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Item_Price,Deli_Price,Expense_Fees,Deli_Type,Deli_Men,Total_Amount,Payment_Status,Deli_Status,Expense_Status,Assign_Remark,Business_Name,Create_Date,Pending_Date,IsDeleted)Values(@co_id,@receiver_name,@c_name,@t_name,@g_name,@g_price,@item_price,@deli_price,@expense_fees,@deli_type,@deli_men,@total_amount,@payment_status,@deli_status,@expense_status,@assign_remark,@business_name,@create_date,@pending_date,@isdeleted)", con);
                create_cmd.Parameters.AddWithValue("@co_id",collectAssignOrderModel.CO_ID);
                create_cmd.Parameters.AddWithValue("@receiver_name", collectAssignOrderModel.Receiver_Name);
                if (collectAssignOrderModel.C_Name == null)
                {
                    create_cmd.Parameters.AddWithValue("@c_name", "");
                }
                else
                {
                    create_cmd.Parameters.AddWithValue("@c_name", collectAssignOrderModel.C_Name);
                }
                if (collectAssignOrderModel.T_Name == null)
                {
                    create_cmd.Parameters.AddWithValue("@t_name", "");
                }
                else
                {
                    create_cmd.Parameters.AddWithValue("@t_name", collectAssignOrderModel.T_Name);
                }
                if (collectAssignOrderModel.G_Name == null)
                {
                    create_cmd.Parameters.AddWithValue("@g_name", "");
                }
                else
                {
                    create_cmd.Parameters.AddWithValue("@g_name", collectAssignOrderModel.G_Name);
                }
                create_cmd.Parameters.AddWithValue("@g_price", collectAssignOrderModel.G_Price);
                create_cmd.Parameters.AddWithValue("@item_price", collectAssignOrderModel.Item_Price);
                create_cmd.Parameters.AddWithValue("@deli_price", collectAssignOrderModel.Deli_Price);
                create_cmd.Parameters.AddWithValue("@expense_fees", collectAssignOrderModel.Expense_Fees);
                create_cmd.Parameters.AddWithValue("@deli_type", collectAssignOrderModel.Deli_Type);
                create_cmd.Parameters.AddWithValue("@deli_men", collectAssignOrderModel.Deli_Men);
                create_cmd.Parameters.AddWithValue("@total_amount", collectAssignOrderModel.Total_Amount);
                create_cmd.Parameters.AddWithValue("@payment_status", "Pending");
                //create_cmd.Parameters.AddWithValue("@create_date",DateTime.Now);
                DateTime serverTime = DateTime.Now.Date;
                DateTime _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Myanmar Standard Time");
                if (collectAssignOrderModel.C_Name == "")
                {
                   // create_cmd.Parameters.AddWithValue("@create_date", collectAssignOrderModel.Assign_Date);
                    create_cmd.Parameters.AddWithValue("@deli_status", "Assigned");
                    create_cmd.Parameters.AddWithValue("@pending_date","");
                }
                else
                {
                    create_cmd.Parameters.AddWithValue("@deli_status", "Pending");
                    create_cmd.Parameters.AddWithValue("@pending_date",_localTime.ToString("yyyy-MM-dd"));
                }
                create_cmd.Parameters.AddWithValue("@expense_status","UnPaid");
                create_cmd.Parameters.AddWithValue("@assign_remark",collectAssignOrderModel.Assign_Remark);
                create_cmd.Parameters.AddWithValue("@create_date", _localTime.ToString("yyyy-MM-dd"));
                create_cmd.Parameters.AddWithValue("@business_name", collectAssignOrderModel.Business_Name);
                create_cmd.Parameters.AddWithValue("@isdeleted", '0');
                create_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status201Created, "Order Collected & Assigned");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/order/collectassign")]
        public IActionResult Update_Collect_Assign_Order([FromBody]CollectAssignOrderModel collectAssignOrderModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(collectAssignOrderModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Receiver_Name=@receiver_name,C_Name=@c_name,T_Name=@t_name,G_Name=@g_name,G_Price=@g_price,Item_Price=@item_price,Deli_Price=@deli_price,Expense_Fees=@expense_fees,Deli_Type=@deli_type,Deli_Men=@deli_men,Total_Amount=@total_amount,Expense_Status=@expense_status,Payment_Status=@payment_status,Assign_Remark=@assign_remark,Update_Date=@update_date Where CAO_ID=@cao_id And Business_Name=@business_name", con);
                update_cmd.Parameters.AddWithValue("@receiver_name", collectAssignOrderModel.Receiver_Name);
                //update_cmd.Parameters.AddWithValue("@create_date", collectAssignOrderModel.Assign_Date);
                update_cmd.Parameters.AddWithValue("@c_name", collectAssignOrderModel.C_Name);
                update_cmd.Parameters.AddWithValue("@t_name", collectAssignOrderModel.T_Name);
                update_cmd.Parameters.AddWithValue("@g_name", collectAssignOrderModel.G_Name);
                update_cmd.Parameters.AddWithValue("@g_price", collectAssignOrderModel.G_Price);
                update_cmd.Parameters.AddWithValue("@item_price", collectAssignOrderModel.Item_Price);
                update_cmd.Parameters.AddWithValue("@deli_price", collectAssignOrderModel.Deli_Price);
                update_cmd.Parameters.AddWithValue("@expense_fees", collectAssignOrderModel.Expense_Fees);
                update_cmd.Parameters.AddWithValue("@deli_type", collectAssignOrderModel.Deli_Type);
                update_cmd.Parameters.AddWithValue("@deli_men", collectAssignOrderModel.Deli_Men);
                update_cmd.Parameters.AddWithValue("@total_amount", collectAssignOrderModel.Total_Amount);
                update_cmd.Parameters.AddWithValue("@expense_status", collectAssignOrderModel.Expense_Status);
                update_cmd.Parameters.AddWithValue("@payment_status", collectAssignOrderModel.Payment_Status);
                update_cmd.Parameters.AddWithValue("@assign_remark", collectAssignOrderModel.Assign_Remark);
                update_cmd.Parameters.AddWithValue("@business_name", collectAssignOrderModel.Business_Name);
                update_cmd.Parameters.AddWithValue("@update_date",DateTime.Now);
                update_cmd.Parameters.AddWithValue("@cao_id", collectAssignOrderModel.CAO_ID);
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Updated Collect Assign Order");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/order/collectassign")]
        public IActionResult Update_Collect_Assign_Order(int CAO_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand delete_cmd = new SqlCommand("Update CollectAssignOrder Set IsDeleted=@isdeleted Where CAO_ID=@cao_id And Business_Name=@business_name", con);
                delete_cmd.Parameters.AddWithValue("@isdeleted", '1');
                delete_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                delete_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Deleted Collect Assign Order");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete
        #region AcceptedWay
        [HttpPut]
        [Route("api/order/collectassign/accepted")]
        public IActionResult Way_Approved(int CAO_ID, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Status=@deli_status,Accept_Date=@accept_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@deli_status", "Accepted");
                update_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                update_cmd.Parameters.AddWithValue("@accept_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted,"Accepted");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion AcceptedWay
    }
}
