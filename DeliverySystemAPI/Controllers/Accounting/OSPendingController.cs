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
    public class OSPendingController : ControllerBase
    {
        private IConfiguration Configuration;
        public OSPendingController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/accounting/ospending")]
        public IActionResult Get_OS_Pending_List(String Client_Name,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //SqlCommand get_cmd = new SqlCommand("Select CollectAssignOrder.CO_ID,CAO_ID,ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) AS SrNo,Client_Name,Receiver_Name,T_Name,Deli_Status,Deli_Type,Item_Price,Half_Paid_Amount,Assign_Remark From CollectAssignOrder Inner Join ClientOrder on ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where CollectAssignOrder.CO_ID in (Select ClientOrder.CO_ID From ClientOrder Where Client_Name Like @client_name And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted) And CollectAssignOrder.CAO_ID in (Select CAO_ID From CollectAssignOrder Where Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Deli_Status='Pending' Or Deli_Status='Assigned' Or Deli_Status='Instock' Or Deli_Status='Rejected' Or Deli_Status='OnWay' And Rejected_Status='Pending') Order By CAO_ID DESC", con);
                SqlCommand get_cmd = new SqlCommand("Select CAO_ID,Client_Name,Receiver_Name,Deli_Men,G_Name,Deli_Status,C_Name,T_Name,Item_Price,Half_Paid_Amount,Deli_Type,Deli_Price,Deli_Price+Item_Price As Total,Deli_Men_Remark,Assign_Remark,CollectAssignOrder.Create_Date,CollectAssignOrder.Pending_Date From CollectAssignOrder Inner Join ClientOrder On (ClientOrder.CO_ID=CollectAssignOrder.CO_ID) Where Client_Name Like @client_name And  Deli_Status <> 'Accepted' And Deli_Status <> 'Completed' And Deli_Status<>'Rejected' And CollectAssignOrder.Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted Order By CollectAssignOrder.Create_Date DESC", con);
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
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get
        #region Update
        [HttpPut]
        [Route("api/accounting/ospending")]
        public IActionResult Update_OS_Pending_List(int CAO_ID,String Deli_Men,String Deli_Status, String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //SqlCommand get_cmd = new SqlCommand("Select CollectAssignOrder.CO_ID,CAO_ID,ROW_NUMBER() OVER (ORDER BY CAO_ID DESC) AS SrNo,Client_Name,Receiver_Name,T_Name,Deli_Status,Deli_Type,Item_Price,Half_Paid_Amount,Assign_Remark From CollectAssignOrder Inner Join ClientOrder on ClientOrder.CO_ID=CollectAssignOrder.CO_ID Where CollectAssignOrder.CO_ID in (Select ClientOrder.CO_ID From ClientOrder Where Client_Name Like @client_name And ClientOrder.Business_Name=@business_name And ClientOrder.IsDeleted=@isdeleted) And CollectAssignOrder.CAO_ID in (Select CAO_ID From CollectAssignOrder Where Business_Name=@business_name And CollectAssignOrder.IsDeleted=@isdeleted And Deli_Status='Pending' Or Deli_Status='Assigned' Or Deli_Status='Instock' Or Deli_Status='Rejected' Or Deli_Status='OnWay' And Rejected_Status='Pending') Order By CAO_ID DESC", con);
                SqlCommand update_cmd = null;
                if (Deli_Status == "Accepted")
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status,Accept_Date=@accept_date Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    update_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                    update_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                    update_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                    update_cmd.Parameters.AddWithValue("@accept_date", DateTime.Now);
                    update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                }
                else
                {
                    update_cmd = new SqlCommand("Update CollectAssignOrder Set Deli_Men=@deli_men,Deli_Status=@deli_status Where CAO_ID=@cao_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                    update_cmd.Parameters.AddWithValue("@cao_id", CAO_ID);
                    update_cmd.Parameters.AddWithValue("@deli_men", Deli_Men);
                    update_cmd.Parameters.AddWithValue("@deli_status", Deli_Status);
                    update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                }
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted,"Updated OS Pending");
            }
            catch (Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
    }
}
