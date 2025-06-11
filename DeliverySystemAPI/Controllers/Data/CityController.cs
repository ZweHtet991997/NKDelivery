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
    public class CityController : ControllerBase
    {
        private IConfiguration Configuration;
        public CityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/data/city")]    
        public IActionResult Get_City(String Business_Name)
        {
            try
            {
                CityModel cityModel = new CityModel();
                string connection = Configuration.GetConnectionString(cityModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_city_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY C_ID DESC) AS  SrNo,C_ID, C_Name,Create_Date,Update_Date From City Where Business_Name=@business_name And IsDeleted=@is_deleted Order By C_ID DESC", con);
                get_city_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_city_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_city_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt,Formatting.None));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
         
        }
        #endregion Get
        #region Get_C_Name
        [HttpGet]
        [Route("api/data/city/price")]
        public IActionResult Get_C_Name(String Business_Name)
        {
            try
            {
                CityModel cityModel = new CityModel();
                string connection = Configuration.GetConnectionString(cityModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_city_cmd = new SqlCommand("SELECT DISTINCT(C_Name) From Price Where Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_city_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_city_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_city_cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                GC.Collect();
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(dt, Formatting.None));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }
        #endregion Get_C_Name
        #region Create
        [HttpPost]
        [Route("api/data/city")]
        public IActionResult Create_City([FromBody]CityModel cityModel)
        {
            string connection = Configuration.GetConnectionString(cityModel.DB_Name);
            try
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //check duplicate city 
                SqlCommand get_city_cmd = new SqlCommand("Select C_ID from City Where C_Name=@c_name And Business_Name=@business_name And IsDeleted=@isDeleted", con);
                get_city_cmd.Parameters.AddWithValue("@c_name", cityModel.C_Name);
                get_city_cmd.Parameters.AddWithValue("@business_name", cityModel.Business_Name);
                get_city_cmd.Parameters.AddWithValue("@isDeleted",'0');
                SqlDataReader reader = get_city_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status409Conflict, cityModel.C_Name + " City Already Exist");
                }
                else
                {
                    reader.Close();
                    SqlCommand create_city_cmd = new SqlCommand("Insert Into City(C_Name,Business_Name,Create_Date,IsDeleted)Values(@c_name,@business_name,@create_date,@isdeleted)", con);
                    create_city_cmd.Parameters.AddWithValue("@c_name", cityModel.C_Name);
                    create_city_cmd.Parameters.AddWithValue("@business_name", cityModel.Business_Name);
                    create_city_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_city_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    create_city_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "City Craeted.");
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
        [Route("api/data/city")]
        public IActionResult Update_City([FromBody]CityModel cityModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(cityModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //check c_id is deleted or not
                SqlCommand get_city_cmd = new SqlCommand("Select C_ID from City Where C_Name=@c_name And IsDeleted=@is_deleted And Business_Name=@business_name ", con);
                get_city_cmd.Parameters.AddWithValue("@is_deleted", '0');
                get_city_cmd.Parameters.AddWithValue("@business_name", cityModel.Business_Name);
                get_city_cmd.Parameters.AddWithValue("@c_name", cityModel.C_Name);
                SqlDataReader reader = get_city_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //the data want to update is already exist
                    reader.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status409Conflict, "Cannot Update the same data.");
                }
                else
                {
                    reader.Close();
                    SqlCommand update_city_cmd = new SqlCommand("Update City Set C_Name=@c_name,Update_Date=@update_date Where C_ID=@c_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    update_city_cmd.Parameters.AddWithValue("@c_id", cityModel.C_ID);
                    update_city_cmd.Parameters.AddWithValue("@c_name", cityModel.C_Name);
                    update_city_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_city_cmd.Parameters.AddWithValue("@business_name", cityModel.Business_Name);
                    update_city_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    update_city_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "City Name Updated");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/city")]
        public IActionResult Delete_City(String C_ID,String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //check the data exist and not deleted
                SqlCommand get_city_cmd = new SqlCommand("Select C_ID From City Where C_ID=@c_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_city_cmd.Parameters.AddWithValue("@c_id", C_ID);
                get_city_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_city_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_city_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //start deleting city
                    reader.Close();
                    SqlCommand delete_city_cmd = new SqlCommand("Delete From City Where C_ID=@c_id And Business_Name=@business_name", con);
                    delete_city_cmd.Parameters.AddWithValue("@c_id", C_ID);
                    delete_city_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_city_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted,"City Deleted.");
                }
                else
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status406NotAcceptable, "City Not Exist OR It's Already Deleted.");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete

    }
}
