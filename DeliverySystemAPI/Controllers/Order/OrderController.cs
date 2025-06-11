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

namespace DeliverySystemAPI.Controllers
{
    public class OrderController : ControllerBase
    {
        private IConfiguration Configuration;
        public OrderController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region GetOrderData
        [HttpGet]
        [Route("api/order")]
        public IActionResult Get_Order_Data(String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_order_cmd = new SqlCommand("Select Order_ID,Client_Name,Deli_Assign_Date,Deli_Township,Client_Payment,Order_Quantity,Order_Amount,Deli_Fees,Expense_Fees,Receiver_Payment,Total_Amount,User_Name,Order_Remark From OrderData Where Business_Name=@business_name And IsDeleted=@isdeleted", con);
                get_order_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_order_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_order_cmd);
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
        #endregion GetOrderData
        #region CreateOrder
        [HttpPost]
        [Route("api/order")]
        public IActionResult Create_Order([FromBody]OrderModel orderModel)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand create_order_cmd = new SqlCommand("Insert Into OrderData(Client_Name,Deli_Assign_Date,Deli_Township,Client_Payment,Order_Quantity,Order_Amount,Deli_Fees,Expense_Fees,Receiver_Payment,Total_Amount,User_Name,Order_Remark,Create_Date,Business_Name,IsDeleted)Values(@client_name,@deli_assign_date,@deli_township,@client_payment,@order_quantity,@order_amount,@deli_fees,@expense_fees,@receiver_payment,@total_amount,@user_name,@order_remark,@create_date,@business_name,@isdeleted)", con);
                create_order_cmd.Parameters.AddWithValue("@client_name",orderModel.Client_Name);
                create_order_cmd.Parameters.AddWithValue("@deli_assign_date", orderModel.Deli_Assign_Date);
                create_order_cmd.Parameters.AddWithValue("@deli_township", orderModel.Deli_Township);
                create_order_cmd.Parameters.AddWithValue("@client_payment", orderModel.Client_Payment);
                create_order_cmd.Parameters.AddWithValue("@order_quantity", orderModel.Order_Quantity);
                create_order_cmd.Parameters.AddWithValue("@order_amount", orderModel.Order_Amount);
                create_order_cmd.Parameters.AddWithValue("@deli_fees", orderModel.Deli_Fees);
                create_order_cmd.Parameters.AddWithValue("@expense_fees", orderModel.Expense_Fees);
                create_order_cmd.Parameters.AddWithValue("@receiver_payment", orderModel.Receiver_Payment);
                create_order_cmd.Parameters.AddWithValue("@total_amount", orderModel.Total_Amount);
                create_order_cmd.Parameters.AddWithValue("@user_name", orderModel.User_Name);
                create_order_cmd.Parameters.AddWithValue("@order_remark", orderModel.Order_Remark);
                create_order_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                create_order_cmd.Parameters.AddWithValue("@business_name", orderModel.Business_Name);
                create_order_cmd.Parameters.AddWithValue("@isdeleted",'0');
                create_order_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status201Created, "Order Created");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion CreateOrder
        #region DeleteOrder
        [HttpDelete]
        [Route("api/order")]
        public IActionResult Delete_City(String Order_ID, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //check the data exist and not deleted
                SqlCommand get_city_cmd = new SqlCommand("Select Order_ID From OrderData Where Order_ID=@order_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_city_cmd.Parameters.AddWithValue("@order_id", Order_ID);
                get_city_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_city_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_city_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //start deleting order
                    reader.Close();
                    SqlCommand delete_city_cmd = new SqlCommand("Delete From OrderData Where Order_ID=@order_id And Business_Name=@business_name", con);
                    delete_city_cmd.Parameters.AddWithValue("@order_id", Order_ID);
                    delete_city_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_city_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Order Deleted.");
                }
                else
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Order Not Exist OR It's Already Deleted.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion DeleteOrder
        #region CreateCollectOrder
        [HttpPut]
        [Route("api/order/collect")]
        public IActionResult Create_Collect_Order([FromBody] OrderCollectModel orderCollectModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(orderCollectModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand create_collect_order_cmd = new SqlCommand("Update OrderData Set Collect_Quantity=@collect_quantity,Collect_Amount=@collect_amount,Collect_Remark=@collect_remark,Order_Status=@order_status,Amount_Difference=@amount_difference,Client_Paid_Amount=@client_paid_amount,Client_Payment_Status=@client_payment_status Where Order_ID=@order_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                create_collect_order_cmd.Parameters.AddWithValue("@collect_quantity", orderCollectModel.Collect_Quantity);
                create_collect_order_cmd.Parameters.AddWithValue("@collect_amount", orderCollectModel.Collect_Amount);
                create_collect_order_cmd.Parameters.AddWithValue("@collect_remark", orderCollectModel.Collect_Remark);
                create_collect_order_cmd.Parameters.AddWithValue("@amount_difference", orderCollectModel.Amount_Difference);
                create_collect_order_cmd.Parameters.AddWithValue("@client_paid_amount", orderCollectModel.Client_Paid_Amount);
                create_collect_order_cmd.Parameters.AddWithValue("@client_payment_status", orderCollectModel.Client_Payment_Status);
                create_collect_order_cmd.Parameters.AddWithValue("@order_status", orderCollectModel.Order_Status);
                create_collect_order_cmd.Parameters.AddWithValue("@order_id", orderCollectModel.Order_ID);
                create_collect_order_cmd.Parameters.AddWithValue("@business_name", orderCollectModel.Business_Name);
                create_collect_order_cmd.Parameters.AddWithValue("@isdeleted", '0');
                create_collect_order_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status201Created, "Collect Order Added");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion CreateCollectOrder
        #region AssignDelivery
        [HttpPut]
        [Route("api/order/assign")]
        public IActionResult Assign_Delivery([FromBody] OrderAssignModel orderAssignModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(orderAssignModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //going to update the row
                SqlCommand create_assign_order_cmd = new SqlCommand("Update OrderData Set User_Name=@user_name,Receiver_Name=@receiver_name,Receiver_Address=@receiver_address,Receiver_Phone_Number=@receiver_phone_number,Receiver_Payment=@receiver_payment,Receiver_Payment_Amount=@receiver_payment_amount,Delivery_Fees=@delivery_fees,Expense_Fees=@expense_fees,Delivery_Assign_Date=@delivery_assign_date,Expense_Status=@expense_status,Delivery_Status=@delivery_status Where Order_ID=@order_id And Business_Name=@business_Name And IsDeleted=@isdeleted", con);
                create_assign_order_cmd.Parameters.AddWithValue("@user_name", orderAssignModel.User_Name);
                create_assign_order_cmd.Parameters.AddWithValue("@receiver_name", orderAssignModel.Receiver_Name);
                create_assign_order_cmd.Parameters.AddWithValue("@receiver_address", orderAssignModel.Receiver_Address);
                create_assign_order_cmd.Parameters.AddWithValue("@receiver_phone_number", orderAssignModel.Receiver_Phone_Number);
                create_assign_order_cmd.Parameters.AddWithValue("@receiver_payment", orderAssignModel.Receiver_Payment);
                create_assign_order_cmd.Parameters.AddWithValue("@receiver_payment_amount", orderAssignModel.Receiver_Payment_Amount);
                create_assign_order_cmd.Parameters.AddWithValue("@delivery_fees", orderAssignModel.Delivery_Fees);
                create_assign_order_cmd.Parameters.AddWithValue("@expense_fees", orderAssignModel.Expense_Fees);
                create_assign_order_cmd.Parameters.AddWithValue("@delivery_assign_date", orderAssignModel.Delivery_Assign_Date);
                create_assign_order_cmd.Parameters.AddWithValue("@expense_status", "Unpaid");
                create_assign_order_cmd.Parameters.AddWithValue("@delivery_status","Assigned");
                create_assign_order_cmd.Parameters.AddWithValue("@order_id", orderAssignModel.Order_ID);
                create_assign_order_cmd.Parameters.AddWithValue("@business_name", orderAssignModel.Business_Name);
                create_assign_order_cmd.Parameters.AddWithValue("@isdeleted", '0');
                create_assign_order_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status201Created, "Order Assigned Successfully!");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion AssignDelivery
    }
}
