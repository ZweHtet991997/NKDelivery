using DeliverySystemAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System;

namespace DeliverySystemAPI.Controllers.Accounting
{
    public class ReturnListController : ControllerBase
    {
        private IConfiguration Configuration;
        public ReturnListController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get_Return_Item_List
        [HttpGet]
        [Route("api/accounting/returnitem")]
        public IActionResult Get_Return_Item_Data(String Client_Name, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select ClientOrder.CO_ID,CAO_ID,Client_Name,Receiver_Name,C_Name,T_Name,Deli_Price,Expense_Fees,Return_Item_Amount,Deli_Men_Remark,Assign_Remark,Pending_Date,CollectAssignOrder.Create_Date,Complete_Date From CollectAssignOrder Left Join ClientOrder On (ClientOrder.CO_ID=CollectAssignOrder.CO_ID) Where Rejected_Status<>'Completed' And Deli_Type='Return Item' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Client_Name Like @client_name", con);
                get_cmd.CommandType = CommandType.Text;
                get_cmd.Parameters.AddWithValue("@client_name", Client_Name);
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
        #endregion Get_Return_Item_list
    }
}
