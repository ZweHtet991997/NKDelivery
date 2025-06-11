using DeliverySystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Controllers.Dashboard
{
    public class WayOverviewController : ControllerBase
    {
        private IConfiguration Configuration;
        public WayOverviewController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/dashboard/way")]
        public IActionResult Get_Overview_Way(String Business_Name)
        {
            DefaultModel defaultModel = new DefaultModel();
            string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
            SqlConnection con = new SqlConnection(connection);
            con.Open();
            DataTable dt = new DataTable();
            dt.Columns.Add("Month");
            dt.Columns.Add("Total_Ways");
            dt.Columns.Add("Assigned");
            dt.Columns.Add("Pending");
            dt.Columns.Add("Completed");
            dt.Columns.Add("Rejected");
            SqlCommand get_cmd = null;
            SqlDataReader reader=null;
            int assigned_ways = 0, pending_ways = 0, rejected_ways = 0, completed_ways = 0, total_ways = 0;
            for (int i = 1; i <= 12; i++)
            {
                string month = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                //Total Ways
                    get_cmd = new SqlCommand("Select Count(CAO_ID) As Total_Ways From CollectAssignOrder Where CO_ID in (Select CO_ID From ClientOrder Where cast([Create_Date] as date) between @from_date And @to_date And Business_Name=@business_name And IsDeleted=@isdeleted) And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    get_cmd.Parameters.AddWithValue("@from_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-01"));
                    get_cmd.Parameters.AddWithValue("@to_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-" + DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.ToString("yyyy")), i)));
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    reader = get_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        total_ways = Convert.ToInt32(reader["Total_Ways"].ToString());
                    }
                    reader.Close();

                    //Assigned Ways
                    get_cmd = new SqlCommand("Select Count(CAO_ID) As Assigned_Ways From CollectAssignOrder Where Deli_Status=@deli_status And Business_Name=@business_name And IsDeleted=@isdeleted And cast([Create_Date] as date) between @from_date And @to_date", con);
                    get_cmd.Parameters.AddWithValue("@from_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-01"));
                    get_cmd.Parameters.AddWithValue("@to_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-" + DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.ToString("yyyy")), i)));
                    get_cmd.Parameters.AddWithValue("@deli_status", "Assigned");
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    reader = get_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        assigned_ways= Convert.ToInt32(reader["Assigned_Ways"].ToString());
                    }
                    reader.Close();
                    //Completed Ways
                    get_cmd = new SqlCommand("Select Count(CAO_ID) As Completed_Ways From CollectAssignOrder Where Deli_Status=@deli_status And cast([Create_Date] as date) between @from_date And @to_date And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    get_cmd.Parameters.AddWithValue("@from_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-01"));
                    get_cmd.Parameters.AddWithValue("@to_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-" + DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.ToString("yyyy")), i)));
                    get_cmd.Parameters.AddWithValue("@deli_status", "Completed");
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    reader = get_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        completed_ways = Convert.ToInt32(reader["Completed_Ways"].ToString());
                    }
                    reader.Close();
                //Pending Ways
                if (DateTime.Now.ToString("MMMM") == month)
                {
                    get_cmd = new SqlCommand("Select Count(CAO_ID) As Pending_Ways From CollectAssignOrder Where Deli_Status=@deli_status And Create_Date='' And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    get_cmd.Parameters.AddWithValue("@from_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-01"));
                    get_cmd.Parameters.AddWithValue("@to_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-" + DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.ToString("yyyy")), i)));
                    get_cmd.Parameters.AddWithValue("@deli_status", "Pending");
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    reader = get_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        pending_ways = Convert.ToInt32(reader["Pending_Ways"].ToString());
                    }
                    reader.Close();
                }
                //Rejected Ways
                get_cmd = new SqlCommand("Select Count(CAO_ID) As Rejected_Ways From CollectAssignOrder Where Rejected_Status=@deli_status And cast([Rejected_Pending_Date] as date) between @from_date And  @to_date And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    get_cmd.Parameters.AddWithValue("@from_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-01"));
                    get_cmd.Parameters.AddWithValue("@to_date", Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + i.ToString() + "-" + DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.ToString("yyyy")), i)));
                    get_cmd.Parameters.AddWithValue("@deli_status", "Pending");
                    get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    reader = get_cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        rejected_ways = Convert.ToInt32(reader["Rejected_Ways"].ToString());
                    }
                    reader.Close();
                    dt.Rows.Add(month, total_ways, assigned_ways,pending_ways, completed_ways, rejected_ways);
                }            
            con.Close();
            GC.Collect();
            return StatusCode(StatusCodes.Status200OK,JsonConvert.SerializeObject(dt));
        }
    }
}
