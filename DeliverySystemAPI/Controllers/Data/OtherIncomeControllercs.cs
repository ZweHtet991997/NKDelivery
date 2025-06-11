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
    public class OtherIncomeControllercs : ControllerBase
    {
        private IConfiguration Configuration;
        public OtherIncomeControllercs(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/manage/data/otherincome")]
        public IActionResult Get_Other_Income_List(String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY OI_ID ASC) AS  SrNo,OI_ID,Other_Income_Name,Other_Income_Amount,Other_Income_Detail,Create_Date From OtherIncome Where Business_Name=@business_name And IsDeleted=@isdeleted", con);
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
        #region Create
        [HttpPost]
        [Route("api/manage/data/otherincome")]
        public IActionResult Create_Other_Income([FromBody]OtherIncomeModel otherIncomeModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(otherIncomeModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand create_cmd = new SqlCommand("Insert Into OtherIncome (Other_Income_Name,Other_Income_Amount,Other_Income_Detail,Create_Date,Business_Name,IsDeleted) Values (@other_income_name,@other_income_amount,@other_income_detail,@create_date,@business_name,@isdeleted)", con);
                create_cmd.Parameters.AddWithValue("@other_income_name", otherIncomeModel.Other_Income_Name);
                create_cmd.Parameters.AddWithValue("@other_income_amount", otherIncomeModel.Other_Income_Amount);
                create_cmd.Parameters.AddWithValue("@other_income_detail", otherIncomeModel.Other_Income_Detail);
                create_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                create_cmd.Parameters.AddWithValue("@business_name", otherIncomeModel.Business_Name);
                create_cmd.Parameters.AddWithValue("@isdeleted",'0');
                create_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status201Created, "Other Income Created");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Delete
        [HttpDelete]
        [Route("api/manage/data/otherincome")]
        public IActionResult Delete_Other_Income(String OI_ID,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update OtherIncome Set IsDeleted=@isdeleted,Update_Date=@update_date Where OI_ID=@oi_id And Business_Name=@business_name", con);
                update_cmd.Parameters.AddWithValue("@oi_id", OI_ID);
                update_cmd.Parameters.AddWithValue("@isdeleted", '1');
                update_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, "Other Income Deleted");
            }
            catch(Exception ex)
            {
                GC.Collect();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Deleted
        #region Update
        [HttpPut]
        [Route("api/manage/data/otherincome")]
        public IActionResult Update_Other_Income([FromBody]OtherIncomeModel otherIncomeModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(otherIncomeModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand update_cmd = new SqlCommand("Update OtherIncome Set Other_Income_Name=@other_income_name,Other_Income_Amount=@other_income_amount,Other_Income_Detail=@other_income_detail Where OI_ID=@oi_id And Business_Name=@business_name And IsDeleted=@isdeleted", con);
                update_cmd.Parameters.AddWithValue("@oi_id", otherIncomeModel.OI_ID);
                update_cmd.Parameters.AddWithValue("@other_income_name", otherIncomeModel.Other_Income_Name);
                update_cmd.Parameters.AddWithValue("@other_income_amount", otherIncomeModel.Other_Income_Amount);
                update_cmd.Parameters.AddWithValue("@other_income_detail", otherIncomeModel.Other_Income_Detail);
                update_cmd.Parameters.AddWithValue("@business_name", otherIncomeModel.Business_Name);
                update_cmd.Parameters.AddWithValue("@isdeleted", '0');
                update_cmd.ExecuteNonQuery();
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status202Accepted, "Updated Other Income");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());
            }           
        }
        #endregion Update
    }
}
