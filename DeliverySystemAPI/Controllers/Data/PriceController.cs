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
    public class PriceController : ControllerBase
    {
        private IConfiguration Configuration;
        public PriceController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region GetAllPrice
        [HttpGet]
        [Route("api/data/price")]
        public IActionResult Get_Price(String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_price_cmd = new SqlCommand("Select ROW_NUMBER() OVER (ORDER BY P_ID DESC) AS  SrNo,P_ID,Deli_Price,Expense_Fees,C_Name,T_Name,Create_Date,Update_Date from Price Where Business_Name=@business_name And IsDeleted=@is_deleted Order By P_ID DESC", con);
                get_price_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_price_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_price_cmd);
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
        #endregion GetAllPrice

        #region GetDeliExpenseFees
        [HttpGet]
        [Route("api/data/fees")]
        public IActionResult Get_Deli_Expense_Fees(String C_Name,String T_Name,String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_price_cmd;
                if (T_Name == "null")
                {
                    get_price_cmd = new SqlCommand("Select Deli_Price,Expense_Fees from Price  Where C_Name Like @c_name And T_Name Like @t_name And Business_Name=@business_name And IsDeleted=@is_deleted Order By P_ID DESC", con);
                    get_price_cmd.Parameters.AddWithValue("@t_name", "%");
                }
                else
                {
                    get_price_cmd = new SqlCommand("Select Deli_Price,Expense_Fees from Price Where C_Name Like @c_name And T_Name =@t_name And Business_Name=@business_name And IsDeleted=@is_deleted Order By p_ID DESC", con);
                    get_price_cmd.Parameters.AddWithValue("@t_name", T_Name);
                }
                get_price_cmd.Parameters.AddWithValue("@c_name", C_Name);
                get_price_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_price_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_price_cmd);
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
        #endregion GetDeliExpenseFees
        #region Get_Township_List
        [HttpGet]
        [Route("api/price/township")]
        public IActionResult Get_Township_List(String Business_Name)
        {
            try
            {
                DefaultModel dm = new DefaultModel();
                string connection = Configuration.GetConnectionString(dm.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_cmd = new SqlCommand("Select Distinct(T_Name) From Price Where T_Name IS NOT NULL And Business_Name=@business_name And IsDeleted=@isdeleted ", con);
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
                return StatusCode(StatusCodes.Status500InternalServerError,ex.ToString());

            }
        }
        #endregion Get_Township_List
        #region Create
        [HttpPost]
        [Route("api/data/price")]
        public IActionResult Create_Price([FromBody] PriceModel priceModel)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                    //start creating price
                    SqlCommand create_price_cmd = new SqlCommand("Insert Into Price(Deli_Price,Expense_Fees,C_Name,T_Name,Business_Name,Create_Date,IsDeleted)Values(@deli_price,@expense_fees,@C_name,@t_name,@business_name,@create_date,@is_deleted)", con);
                    create_price_cmd.Parameters.AddWithValue("@deli_price", priceModel.Deli_Price);
                    create_price_cmd.Parameters.AddWithValue("@expense_fees", priceModel.Expense_Fees);
                    create_price_cmd.Parameters.AddWithValue("@c_name", priceModel.C_Name);
                    if (priceModel.T_Name != null)
                    {
                        create_price_cmd.Parameters.AddWithValue("@t_name", priceModel.T_Name);
                    }
                    else
                    {
                        create_price_cmd.Parameters.AddWithValue("@t_name","");
                    }
                    create_price_cmd.Parameters.AddWithValue("@business_name", priceModel.Business_Name);
                    create_price_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_price_cmd.Parameters.AddWithValue("is_deleted", '0');
                    create_price_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "Price Created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/data/price")]
        public IActionResult Update_Price([FromBody] PriceModel priceModel)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                //no need to detect data already exist or not because it is deli price.
                //start updating price
                    SqlCommand update_price_cmd = new SqlCommand("Update Price Set Deli_Price=@deli_price,Expense_Fees=@expense_fees,C_Name=@c_name,T_Name=@t_name,Update_Date=@update_date Where P_ID=@p_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    update_price_cmd.Parameters.AddWithValue("@p_id", priceModel.P_ID);
                    update_price_cmd.Parameters.AddWithValue("@deli_price", priceModel.Deli_Price);
                    update_price_cmd.Parameters.AddWithValue("@expense_fees", priceModel.Expense_Fees);
                    update_price_cmd.Parameters.AddWithValue("@c_name", priceModel.C_Name);
                    update_price_cmd.Parameters.AddWithValue("@t_name", priceModel.T_Name);
                    update_price_cmd.Parameters.AddWithValue("@business_name", priceModel.Business_Name);
                    update_price_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_price_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    update_price_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Price Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/price")]
        public IActionResult Delete_Price(String P_ID, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_price_cmd = new SqlCommand("Select Deli_Price From Price Where P_ID=@p_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                get_price_cmd.Parameters.AddWithValue("@p_id", P_ID);
                get_price_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_price_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataReader reader = get_price_cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    //start deleting price
                    reader.Close();
                    SqlCommand delete_price_cmd = new SqlCommand("Delete From Price Where P_ID=@p_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    delete_price_cmd.Parameters.AddWithValue("@p_id", P_ID);
                    delete_price_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_price_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    delete_price_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Price Deleted");
                }
                else
                {
                    reader.Close();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status406NotAcceptable, "Price Not Exist OR It's Already Deleted.");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete
    }
}
