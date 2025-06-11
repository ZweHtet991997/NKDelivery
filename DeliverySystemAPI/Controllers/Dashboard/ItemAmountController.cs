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

namespace DeliverySystemAPI.Controllers.Dashboard
{
    public class ItemAmountController : ControllerBase
    {
        private IConfiguration Configuration;
        public ItemAmountController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/dashboard/itemamount")]
        public IActionResult Get_Item_Amount(String Business_Name)
        {
            DefaultModel defaultModel = new DefaultModel();
            string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            var city_onway_item_amount = ""; var city_pending_item_amount=""; var city_assigned_item_amount=""; var city_completed_item_amount = ""; var township_onway_item_amount = ""; var township_pending_item_amount = ""; var township_assigned_item_amount=""; var township_completed_item_amount = "";
            SqlCommand get_cmd = new SqlCommand("Select SUM(Item_Price) As City_OnWay_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name<>'' And CollectAssignOrder.T_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "OnWay");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            SqlDataReader reader = null;
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                city_onway_item_amount =reader["City_OnWay_Item_Amount"].ToString();
            }
            reader.Close();
            get_cmd = new SqlCommand("Select SUM(Item_Price) As City_Pending_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name<>'' And CollectAssignOrder.T_Name='' And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "Pending");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                city_pending_item_amount = reader["City_Pending_Item_Amount"].ToString();
            }
            reader.Close();
            //get_cmd = new SqlCommand("Select SUM(Item_Price) As City_Assigned_Item_Amount From CollectAssignOrder Where  C_Name<>'' And T_Name='' And Deli_Status=@deli_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd = new SqlCommand("Select SUM(Item_Price) As City_Assigned_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name<>'' And CollectAssignOrder.T_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "Assigned");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                city_assigned_item_amount =reader["City_Assigned_Item_Amount"].ToString();
            }
            reader.Close();
            //get_cmd = new SqlCommand("Select SUM(Item_Price) As City_Completed_Item_Amount From CollectAssignOrder Where  C_Name<>'' And T_Name='' And  Deli_Status=@deli_status And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd = new SqlCommand("Select SUM(Item_Price) As City_Completed_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.C_Name<>'' And CollectAssignOrder.T_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "Completed");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                city_completed_item_amount = reader["City_Completed_Item_Amount"].ToString();
            }
            reader.Close();
            //get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_OnWay_Item_Amount From CollectAssignOrder Where Deli_Status=@deli_status And T_Name<>'' And C_Name='' And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_OnWay_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.T_Name<>'' And CollectAssignOrder.C_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "OnWay");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                township_onway_item_amount = reader["Township_OnWay_Item_Amount"].ToString();
            }
            reader.Close();
            reader.Close();
            //get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_Pending_Item_Amount From CollectAssignOrder Where Deli_Status=@deli_status And T_Name<>'' And C_Name='' And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_Pending_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.T_Name<>'' And CollectAssignOrder.C_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "Pending");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                township_pending_item_amount = reader["Township_Pending_Item_Amount"].ToString();
            }
            reader.Close();
            reader.Close();
            //get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_Assigned_Item_Amount From CollectAssignOrder Where Deli_Status=@deli_status And T_Name<>'' And C_Name='' And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_Assigned_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.T_Name<>'' And CollectAssignOrder.C_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "Assigned");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                township_assigned_item_amount =reader["Township_Assigned_Item_Amount"].ToString();
            }
            reader.Close();
            reader.Close();
            //get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_Completed_Item_Amount From CollectAssignOrder Where Deli_Status=@deli_status And T_Name<>'' And C_Name='' And Business_Name=@business_name And IsDeleted=@isdeleted", con);
            get_cmd = new SqlCommand("Select SUM(Item_Price) As Township_Completed_Item_Amount From CollectAssignOrder Inner Join Users On(Users.User_Name=CollectAssignOrder.Deli_Men) Where CollectAssignOrder.T_Name<>'' And CollectAssignOrder.C_Name=''  And Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Users.Business_Name=@business_name And Users.IsDeleted=@isdeleted", con);
            get_cmd.Parameters.AddWithValue("@deli_status", "Completed");
            get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
            get_cmd.Parameters.AddWithValue("@isdeleted", '0');
            reader = get_cmd.ExecuteReader();
            while (reader.Read())
            {
                township_completed_item_amount = reader["Township_Completed_Item_Amount"].ToString();
            }
            reader.Close();
            DataTable dt = new DataTable();
            dt.Columns.Add("City_OnWay_Item_Amount");
            dt.Columns.Add("City_Pending_Item_Amount");
            dt.Columns.Add("City_Assigned_Item_Amount");
            dt.Columns.Add("City_Completed_Item_Amount");
            dt.Columns.Add("Township_OnWay_Item_Amount");
            dt.Columns.Add("Township_Pending_Item_Amount");
            dt.Columns.Add("Township_Assigned_Item_Amount");
            dt.Columns.Add("Township_Completed_Item_Amount");
            dt.Rows.Add(city_onway_item_amount,city_pending_item_amount, city_assigned_item_amount, city_completed_item_amount, township_onway_item_amount, township_pending_item_amount,township_assigned_item_amount, township_completed_item_amount);
            con.Close();
            GC.Collect();
            return StatusCode(StatusCodes.Status200OK,JsonConvert.SerializeObject(dt));
        }
    }
}
