using DeliverySystemAPI.Model;
using DeliverySystemAPI.Services.OverAll;
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
    public class ClientOrderController : ControllerBase
    {
        private IConfiguration Configuration;
        public ClientOrderController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/order/client")]
        public IActionResult Get_Client_Order(String Client_Name,String Business_Name,String From_Date,String To_Date)
        {
            try
            {
                DateTime serverTime = DateTime.Now.Date;
                DateTime _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Myanmar Standard Time");
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //SqlCommand get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CO_ID DESC) AS  SrNo,CO_ID,Client_Name,Item_Quantity,Total_Amount,Client_Pay_Type,Pickup_Date,Remark,Create_Date,Update_Date From ClientOrder Where Business_Name=@business_name And IsDeleted=@isdeleted", con);
                SqlCommand get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY ClientOrder.Create_Date ASC) AS  SrNo,ClientOrder.CO_ID,Client_Name,ClientOrder.Item_Quantity,ClientOrder.Total_Amount,Gate_Amount,All_Paid_Amount,Client_Pay_Type,Order_Check_Status,Pickup_Date,Remark,ClientOrder.Pickup_Date,ClientOrder.Create_Date,ClientOrder.Update_Date,SUM(CollectAssignOrder.Item_Price) As Collect_Amount  From ClientOrder Full Join CollectAssignOrder ON ClientOrder.CO_ID=CollectAssignOrder.CO_ID And CollectAssignOrder.IsDeleted=@isdeleted  Where ClientOrder.Client_Name Like @client_name And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted And cast([ClientOrder].Create_Date as date) between @from_date and @to_date Group By ClientOrder.CO_ID,ClientOrder.Client_Name,ClientOrder.Create_Date,ClientOrder.Item_Quantity,ClientOrder.Total_Amount,Gate_Amount,All_Paid_Amount,Order_Check_Status,Client_Pay_Type,Pickup_Date,Remark,ClientOrder.Update_Date", con);
                if (From_Date==null && To_Date==null) // means the user want the client order by today date
                {
                    get_cmd.Parameters.AddWithValue("@from_date", _localTime.ToString("yyyy-MM-dd"));
                    get_cmd.Parameters.AddWithValue("@to_date", _localTime.ToString("yyyy-MM-dd"));
                }
                else // mean the user want the client order by filter date
                {
                    get_cmd.Parameters.AddWithValue("@from_date",From_Date);
                    get_cmd.Parameters.AddWithValue("@to_date",To_Date);
                }
                /*                get_cmd.Parameters.AddWithValue("@deli_status","Accepted");*/
                get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return Ok(JsonConvert.SerializeObject(dt));
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
            
        }
        #endregion Get
        #region Create
        [HttpPost]
        [Route("api/order/client")]
        public IActionResult Index([FromBody] ClientOrderModel clientOrderModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(clientOrderModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                DateTime serverTime = DateTime.Now.Date;
                DateTime _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Myanmar Standard Time");
                var createDate = _localTime.ToString("yyyy-MM-dd");
                con.Open();
                //SqlCommand create_cmd = new SqlCommand("Insert Into ClientOrder (Client_Name,Item_Quantity,Pickup_Date,Total_Amount,Gate_Amount,All_Paid_Amount,Client_Pay_Type,Payment_Status,Order_Check_Status,Remark,Business_Name,Create_Date,IsDeleted)Values(@client_name,@item_quantity,@pickup_date,@total_amount,@gate_amount,@all_paid_amount,@client_pay_type,@payment_status,@remark,@business_name,@create_date,@isdeleted)", con);
                
                SqlCommand create_cmd = new SqlCommand("Insert Into ClientOrder (Client_Name,Item_Quantity,Pickup_Date,Total_Amount,Gate_Amount,All_Paid_Amount,Client_Pay_Type,Order_Check_Status,Payment_Status,Remark,Business_Name,Create_Date,IsDeleted)Values(@client_name,@item_quantity,@pickup_date,@total_amount,@gate_amount,@all_paid_amount,@client_pay_type,@order_check_status,@payment_status,@remark,@business_name,@create_date,@isdeleted)", con);
                create_cmd.Parameters.AddWithValue("@client_name", clientOrderModel.Client_Name);
                create_cmd.Parameters.AddWithValue("@item_quantity", clientOrderModel.Item_Quantity);
                create_cmd.Parameters.AddWithValue("@pickup_date", clientOrderModel.Pickup_Date);
                create_cmd.Parameters.AddWithValue("@total_amount", clientOrderModel.Total_Amount);
                create_cmd.Parameters.AddWithValue("@gate_amount", clientOrderModel.Gate_Amount);
                create_cmd.Parameters.AddWithValue("@all_paid_amount", clientOrderModel.All_Paid_Amount);
                create_cmd.Parameters.AddWithValue("@client_pay_type", clientOrderModel.Client_Pay_Type);
                create_cmd.Parameters.AddWithValue("@order_check_status", clientOrderModel.Order_Check_Status);
                create_cmd.Parameters.AddWithValue("@payment_status", "Pending");
                //create_cmd.Parameters.AddWithValue("@order_check_status",clientOrderModel.Order_Check_Status);
                create_cmd.Parameters.AddWithValue("@remark", clientOrderModel.Remark);
                create_cmd.Parameters.AddWithValue("@business_name", clientOrderModel.Business_Name);
                create_cmd.Parameters.AddWithValue("@create_date", createDate);
                create_cmd.Parameters.AddWithValue("@isdeleted", '0');
                create_cmd.ExecuteNonQuery();
                con.Close();
                return StatusCode(StatusCodes.Status201Created, "Created Client Order");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/order/client")]
        public IActionResult Update_Client_Order([FromBody]ClientOrderModel clientOrderModel)
        {
            try
            {
                string connecion = Configuration.GetConnectionString(clientOrderModel.DB_Name);
                SqlConnection con = new SqlConnection(connecion);
                con.Open();
                //SqlCommand update_cmd = new SqlCommand("Update ClientOrder Set Client_Name=@client_Name,Item_Quantity=@item_quantity,Order_Check_Status=@order_check_status,Pickup_Date=@pickup_date,Total_Amount=@total_amount,Gate_Amount=@gate_amount,All_Paid_Amount=@all_paid_amount,Client_Pay_Type=@client_pay_type,Remark=@remark,Update_Date=@update_date Where CO_ID=@co_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                SqlCommand update_cmd = new SqlCommand("Update ClientOrder Set Client_Name=@client_Name,Item_Quantity=@item_quantity,Pickup_Date=@pickup_date,Total_Amount=@total_amount,Gate_Amount=@gate_amount,All_Paid_Amount=@all_paid_amount,Client_Pay_Type=@client_pay_type,Order_Check_Status=@order_check_status,Remark=@remark,Update_Date=@update_date Where CO_ID=@co_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@client_name", clientOrderModel.Client_Name);
                update_cmd.Parameters.AddWithValue("@item_quantity", clientOrderModel.Item_Quantity);                
                update_cmd.Parameters.AddWithValue("@pickup_date", clientOrderModel.Pickup_Date);
                update_cmd.Parameters.AddWithValue("@total_amount", clientOrderModel.Total_Amount);
                update_cmd.Parameters.AddWithValue("@gate_amount", clientOrderModel.Gate_Amount);
                update_cmd.Parameters.AddWithValue("@all_paid_amount", clientOrderModel.All_Paid_Amount);
                update_cmd.Parameters.AddWithValue("@client_pay_type", clientOrderModel.Client_Pay_Type);
                update_cmd.Parameters.AddWithValue("@order_check_status", clientOrderModel.Order_Check_Status);
                update_cmd.Parameters.AddWithValue("@remark", clientOrderModel.Remark);
                update_cmd.Parameters.AddWithValue("@co_id", clientOrderModel.CO_ID);
                update_cmd.Parameters.AddWithValue("@business_name", clientOrderModel.Business_Name);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted,"Client Order Updated");
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
        [Route("api/order/client")]
        public IActionResult Delete_Client_Order(String CO_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connecion = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connecion);
                con.Open();
                //updating IsDelete Column at ClientOrder Table.
                SqlCommand update_delete_cmd = new SqlCommand("Update ClientOrder Set IsDeleted=@isdeleted Where CO_ID=@co_id And Business_Name=@business_name", con);
                update_delete_cmd.Parameters.AddWithValue("@isdeleted", '1');
                update_delete_cmd.Parameters.AddWithValue("@co_id", CO_ID);
                update_delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_delete_cmd.ExecuteNonQuery();
                //Deleting Row from  CollectAssignOrder Table.
                SqlCommand delete_cmd = new SqlCommand("Delete From CollectAssignOrder Where Business_Name=@business_name And IsDeleted=@isdeleted And CO_ID=@co_id", con);
                delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                delete_cmd.Parameters.AddWithValue("@isdeleted", '0');
                delete_cmd.Parameters.AddWithValue("@co_id", CO_ID);
                delete_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Deleted Client Order");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete
        #region Check
        [HttpGet]
        [Route("api/order/client/check")]
        public IActionResult Check_Order_And_Collect_Amount(String CO_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connecion = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connecion);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select SUM(Item_Price) AS Coll_Total_Amount,Gat From CollectAssignOrder Where CO_ID=@co_id And Business_Name=@business_name", con);
                get_cmd.Parameters.AddWithValue("@co_id", CO_ID);
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
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
        #endregion Check
        #region OverAll
        [HttpGet]
        [Route("api/order/overall")]
        public IActionResult Get_OverAll_Data(String CO_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                ClientOrderOverAll call_service = new ClientOrderOverAll();
                Object dt=call_service.Count_CollectAssign_By_COID(CO_ID, Business_Name, con);
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, dt);
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }
        #endregion OverAll
    }
}
