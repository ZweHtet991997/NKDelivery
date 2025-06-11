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
    public class WayCheckController : ControllerBase
    {
        private IConfiguration Configuration;
        public WayCheckController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/accounting/waycheck")]
        public IActionResult Index(String Deli_Men,String From_Date,String To_Date,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SrNo,Deli_Men,Count(CAO_ID) as Total_Ways,Sum(Item_Price) as Total_Item_Amount From CollectAssignOrder Where Deli_Men Like @deli_men And cast([Create_Date] as date)between @from_date And @to_date And Business_Name=@business_name And IsDeleted=@isdeleted Group By Deli_Men Order By Total_Ways DESC", con);
                get_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                get_cmd.Parameters.AddWithValue("@to_date", To_Date);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
