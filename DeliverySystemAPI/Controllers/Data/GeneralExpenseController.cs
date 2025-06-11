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
    public class GeneralExpenseController : ControllerBase
    {
        private IConfiguration Configuration;
        public GeneralExpenseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/data/gexp")]
        public IActionResult Get_C_Name(String From_Date,String To_Date,String Business_Name)
        {
            try
            {
                GeneralExpenseModel generalExpenseModel = new GeneralExpenseModel();
                string connection = Configuration.GetConnectionString(generalExpenseModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_gexp_cmd = null;
                if (From_Date == null && To_Date == null) // means the user want the client order by today date
                {
                    get_gexp_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY GEXP_ID ASC) AS  SrNo,GEXP_ID,General_Expense_Name,General_Expense_Detail,General_Expense_Amount,Create_Date From GeneralExpense Where Business_Name=@business_name And IsDeleted=@is_deleted", con);
                }
                else
                {
                    get_gexp_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY GEXP_ID ASC) AS  SrNo,GEXP_ID,General_Expense_Name,General_Expense_Detail,General_Expense_Amount,Create_Date From GeneralExpense Where Business_Name=@business_name And IsDeleted=@is_deleted And cast([Create_Date] as date) between @from_date and @to_date", con);
                    get_gexp_cmd.Parameters.AddWithValue("@from_date", From_Date);
                    get_gexp_cmd.Parameters.AddWithValue("@to_date", To_Date);
                }             
                get_gexp_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_gexp_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_gexp_cmd);
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
        #endregion Get
        #region Create
        [HttpPost]
        [Route("api/data/gexp")]
        public IActionResult Create_General_Expense([FromBody] GeneralExpenseModel generalExpenseModel)
        {
            string connection = Configuration.GetConnectionString(generalExpenseModel.DB_Name);
            try
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();           
                    SqlCommand create_gexp_cmd = new SqlCommand("Insert Into GeneralExpense(General_Expense_Name,General_Expense_Detail,General_Expense_Amount,Business_Name,Create_Date,IsDeleted)Values(@general_expense_name,@general_expense_detail,@general_expense_amount,@business_name,@create_date,@isdeleted)", con);
                    create_gexp_cmd.Parameters.AddWithValue("@general_expense_name", generalExpenseModel.General_Expense_Name);
                    create_gexp_cmd.Parameters.AddWithValue("@general_expense_detail", generalExpenseModel.General_Expense_Detail);
                    create_gexp_cmd.Parameters.AddWithValue("@general_expense_amount", generalExpenseModel.General_Expense_Amount);
                    create_gexp_cmd.Parameters.AddWithValue("@business_name", generalExpenseModel.Business_Name);
                    create_gexp_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_gexp_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    create_gexp_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "General Expense Craeted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/data/gexp")]
        public IActionResult Update_City([FromBody] GeneralExpenseModel generalExpenseModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(generalExpenseModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();                             
                    SqlCommand update_cmd = new SqlCommand("Update GeneralExpense Set General_Expense_Name=@general_expense_name,General_Expense_Detail=@general_expense_detail,General_Expense_Amount=@general_expense_Amount,Update_Date=@update_date,Create_Date=@create_date Where GEXP_ID=@gexp_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    update_cmd.Parameters.AddWithValue("@general_expense_name", generalExpenseModel.General_Expense_Name);
                    update_cmd.Parameters.AddWithValue("@general_expense_detail", generalExpenseModel.General_Expense_Detail);
                    update_cmd.Parameters.AddWithValue("@general_expense_amount", generalExpenseModel.General_Expense_Amount);
                    update_cmd.Parameters.AddWithValue("@gexp_id", generalExpenseModel.GEXP_ID);
                    update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_cmd.Parameters.AddWithValue("@create_date", generalExpenseModel.Create_Date);
                    update_cmd.Parameters.AddWithValue("@business_name", generalExpenseModel.Business_Name);
                    update_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    update_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "General Expense  Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/gexp")]
        public IActionResult Delete_City(String GEXP_ID, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                    SqlCommand delete_cmd = new SqlCommand("Delete From GeneralExpense Where GEXP_ID=@gexp_id And Business_Name=@business_name", con);
                    delete_cmd.Parameters.AddWithValue("@gexp_id", GEXP_ID);
                    delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "General Expense Deleted.");                              
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete

    }
}
