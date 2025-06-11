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
    public class TownshipController : ControllerBase
    {
        private IConfiguration Configuration;
        public TownshipController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/data/township")]
        public IActionResult Get_Township(String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_township_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY T_ID DESC) AS SrNo,T_ID,C_Name,T_Name,Deli_Price,Expense_Fees,Create_Date,Update_Date FROM Township Where Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_township_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_township_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_township_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt).ToString());
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Get
        #region Create
        [HttpPost]
        [Route("api/data/township")]
        public IActionResult Create_Township([FromBody]TownshipModel townshipModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(townshipModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_township_cmd = new SqlCommand("Select T_ID From Township Where T_Name=@t_name And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_township_cmd.Parameters.AddWithValue("@t_name", townshipModel.T_Name);
                get_township_cmd.Parameters.AddWithValue("@business_name", townshipModel.Business_Name);
                get_township_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_township_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //data already exist
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status409Conflict, townshipModel.T_Name + " Township Already Exist.");
                }
                else
                {
                    //start creating township
                    reader.Close();
                    SqlCommand create_township_cmd = new SqlCommand("Insert Into Township(T_Name,C_Name,Deli_Price,Expense_Fees,Business_Name,Create_Date,IsDeleted)Values(@t_name,@c_id,@deli_price,@expense_fees,@business_name,@create_date,@is_deleted)", con);
                    create_township_cmd.Parameters.AddWithValue("@t_name", townshipModel.T_Name);
                    create_township_cmd.Parameters.AddWithValue("@c_id", townshipModel.C_Name);
                    create_township_cmd.Parameters.AddWithValue("@deli_price", townshipModel.Deli_Price);
                    create_township_cmd.Parameters.AddWithValue("@expense_fees", townshipModel.Expense_Fees);
                    create_township_cmd.Parameters.AddWithValue("@business_name", townshipModel.Business_Name);
                    create_township_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_township_cmd.Parameters.AddWithValue("is_deleted", '0');
                    create_township_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "Township Created");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());  
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/data/township")]
        public IActionResult Update_Township([FromBody]TownshipModel townshipModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(townshipModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                    //cannot check whether already exist or not before updating because sql cannot detect case sensitive.
                    //start updating township
                    SqlCommand update_township_cmd = new SqlCommand("Update Township Set T_Name=@t_name,C_Name=@c_name,Deli_Price=@deli_price,Expense_Fees=@expense_fees,Update_Date=@update_date Where T_ID=@t_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    update_township_cmd.Parameters.AddWithValue("@t_id", townshipModel.T_ID);
                    update_township_cmd.Parameters.AddWithValue("@t_name", townshipModel.T_Name);
                    update_township_cmd.Parameters.AddWithValue("@c_name", townshipModel.C_Name);
                    update_township_cmd.Parameters.AddWithValue("@deli_price", townshipModel.Deli_Price);
                    update_township_cmd.Parameters.AddWithValue("@expense_fees", townshipModel.Expense_Fees);
                    update_township_cmd.Parameters.AddWithValue("@business_name", townshipModel.Business_Name);
                    update_township_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_township_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    update_township_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Township Updated");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }

        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/township")]
        public IActionResult Delete_Township(String T_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_township_cmd = new SqlCommand("Select T_Name From Township Where T_ID=@t_id And Business_Name=@business_name And IsDeleted=@is_deleted",con);
                get_township_cmd.Parameters.AddWithValue("@t_id", T_ID);
                get_township_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_township_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_township_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //start deleting township
                    reader.Close();
                    SqlCommand delete_township_cmd = new SqlCommand("Delete From Township Where T_ID=@t_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    delete_township_cmd.Parameters.AddWithValue("@t_id", T_ID);
                    delete_township_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_township_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    delete_township_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Township Deleted");
                }
                else
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Township Not Exist OR It's Already Deleted.");

                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }
        }
        #endregion Delete
    }
}
