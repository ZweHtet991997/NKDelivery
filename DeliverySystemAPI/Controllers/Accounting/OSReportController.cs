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
    public class OSReportController : ControllerBase
    {
        private IConfiguration Configuration;
        public OSReportController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        [Route("api/accounting/osreport")]
        public IActionResult Index(String Client_Name,String From_Date,String To_Date,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY CAO_ID ASC) AS SrNo,CAO_ID,CollectAssignOrder.CO_ID,Receiver_Name,C_Name,T_Name,G_Name,G_Price,Item_Price,Deli_Price,Expense_Fees,Deli_Type,Deli_Men,CollectAssignOrder.Total_Amount,CollectAssignOrder.Payment_Status,Deli_Status,Expense_Status,Assign_Remark,CollectAssignOrder.Create_Date,Accept_Date From CollectAssignOrder Inner Join ClientOrder On (ClientOrder.CO_ID=CollectAssignOrder.CO_ID) Where Client_Name Like @client_name And cast(ClientOrder.[Create_Date]as date)between @from_date And @to_date And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted And CollectAssignOrder.IsDeleted=@isdeleted", con);
                get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
                get_cmd.Parameters.AddWithValue("@from_date", From_Date);
                get_cmd.Parameters.AddWithValue("@to_date", To_Date);
                get_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_cmd.Parameters.AddWithValue("@isdeleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK,JsonConvert.SerializeObject(dt));
            }
            catch(SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
