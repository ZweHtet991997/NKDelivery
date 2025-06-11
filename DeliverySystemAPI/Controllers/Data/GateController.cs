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

namespace DeliverySystemAPI.Controllers.Data
{
    public class GateController : ControllerBase
    {
        private IConfiguration Configuration;
        public GateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/data/gate")]
        public IActionResult Get_Gate(String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_gate_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY G_ID DESC) AS  SrNo,G_ID,Deli_Men,G_Name,G_Price,Expense_Fees,Create_Date,Update_Date from Gate Where Business_Name=@business_name And IsDeleted=@is_deleted Order By G_ID DESC", con);
                get_gate_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_gate_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_gate_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt).ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Get
        #region Create
        [HttpPost]
        [Route("api/data/gate")]
        public IActionResult Create_Gate([FromBody] GateModel gateModel)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_gate_cmd = new SqlCommand("Select G_ID From Gate Where G_Name=@g_name And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_gate_cmd.Parameters.AddWithValue("@g_name", gateModel.G_Name);
                get_gate_cmd.Parameters.AddWithValue("@business_name", gateModel.Business_Name);
                get_gate_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_gate_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //data already exist
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status409Conflict, gateModel.G_Name + " Gate Already Exist.");
                }
                else
                {
                    //start creating gate
                    reader.Close();
                    SqlCommand create_gate_cmd = new SqlCommand("Insert Into Gate(Deli_Men,G_Name,G_Price,Expense_Fees,Business_Name,Create_Date,IsDeleted)Values(@deli_men,@g_name,@g_price,@expense_fees,@business_name,@create_date,@is_deleted)", con);
                    create_gate_cmd.Parameters.AddWithValue("@deli_men", gateModel.Deli_Men);
                    create_gate_cmd.Parameters.AddWithValue("@g_name", gateModel.G_Name);
                    create_gate_cmd.Parameters.AddWithValue("@g_price", gateModel.G_Price);
                    create_gate_cmd.Parameters.AddWithValue("@expense_fees", gateModel.Expense_Fees);
                    create_gate_cmd.Parameters.AddWithValue("@business_name", gateModel.Business_Name);
                    create_gate_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_gate_cmd.Parameters.AddWithValue("is_deleted", '0');
                    create_gate_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "Gate Created");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/data/gate")]
        public IActionResult Update_Gate([FromBody] GateModel gateModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(gateModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //cannot check whether already exist or not before updating because sql cannot detect case sensitive.
                //start updating gate
                SqlCommand update_gate_cmd = new SqlCommand("Update Gate Set Deli_Men=@deli_men,G_Name=@g_name,G_Price=@g_price,Expense_Fees=@expense_fees,Update_Date=@update_date Where G_ID=@g_id And Business_Name=@business_name  And IsDeleted=@is_deleted", con);
                    update_gate_cmd.Parameters.AddWithValue("@g_id", gateModel.G_ID);
                    update_gate_cmd.Parameters.AddWithValue("@deli_men", gateModel.Deli_Men);
                    update_gate_cmd.Parameters.AddWithValue("@g_name", gateModel.G_Name);
                    update_gate_cmd.Parameters.AddWithValue("@g_price", gateModel.G_Price);
                    update_gate_cmd.Parameters.AddWithValue("@expense_fees", gateModel.Expense_Fees);
                    update_gate_cmd.Parameters.AddWithValue("@business_name", gateModel.Business_Name);
                    update_gate_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_gate_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    update_gate_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Gate Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/gate")]
        public IActionResult Delete_Gate(String G_ID, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_gate_cmd = new SqlCommand("Select G_Name From Gate Where G_ID=@g_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_gate_cmd.Parameters.AddWithValue("@g_id", G_ID);
                get_gate_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_gate_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_gate_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //start deleting gate
                    reader.Close();
                    SqlCommand delete_gate_cmd = new SqlCommand("Delete From Gate Where G_ID=@g_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    delete_gate_cmd.Parameters.AddWithValue("@g_id", G_ID);
                    delete_gate_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_gate_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    delete_gate_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Gate Deleted");
                }
                else
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Gate Not Exist OR It's Already Deleted.");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete
        #region GetGate&ExpensePrice
        [HttpGet]
        [Route("api/data/gateprice")]
        public IActionResult Get_Gate_Price(String G_Name,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_gate_cmd = new SqlCommand("SELECT  G_Price,Expense_Fees from Gate Where G_Name=@g_name And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_gate_cmd.Parameters.AddWithValue("@g_name", G_Name);
                get_gate_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_gate_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_gate_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt).ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion GetGatePrice
    }
}
