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
    public class NoDeliMenController : ControllerBase
    {
        private IConfiguration Configuration;
        public NoDeliMenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/accounting/nodelimen")]
        public IActionResult Get_No_Deli_Men_List(String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select CAO_ID,ClientOrder.Client_Name,Receiver_Name,T_Name,CollectAssignOrder.Total_Amount,CollectAssignOrder.Create_Date From CollectAssignOrder Inner Join ClientOrder On ClientOrder.CO_ID=CollectAssignOrder.CO_ID And Deli_Men='' And CollectAssignOrder.Deli_Status=@deli_status And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@deli_status", "Assigned");
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        [HttpDelete]
        [Route("api/accounting/nodelimen")]
        public IActionResult Delete_No_Deli_Men_Way(String CAO_ID,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand delete_cmd = new SqlCommand("Update CollectAssignOrder Set IsDeleted=@isdeleted Where CAO_ID=@cao_id and Business_Name=@business_name", con);
                delete_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                delete_cmd.Parameters.AddWithValue("@isdeleted", '1');
                delete_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Deleted");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
