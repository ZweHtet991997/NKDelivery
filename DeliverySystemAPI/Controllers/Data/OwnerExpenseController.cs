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
    public class OwnerExpenseController : ControllerBase
    {
        private IConfiguration Configuration;
        public OwnerExpenseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Get
        [HttpGet]
        [Route("api/data/oexp")]
        public IActionResult Get_Owner_Expense(String From_Date,String To_Date,String Business_Name)
        {
            try
            {
                OwnerExpenseModel OwnerExpenseModel = new OwnerExpenseModel();
                string connection = Configuration.GetConnectionString(OwnerExpenseModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                SqlCommand get_oexp_cmd = null;
                if (From_Date == null && To_Date == null) // means the user want the client order by today date
                {
                    get_oexp_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY OEXP_ID ASC) AS  SrNo,OEXP_ID,Owner_Expense_Detail,Owner_Expense_Amount,Create_Date From OwnerExpense Where Business_Name=@business_name And IsDeleted=@is_deleted", con);
                }
                else
                {
                    get_oexp_cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (ORDER BY OEXP_ID ASC) AS  SrNo,OEXP_ID,Owner_Expense_Detail,Owner_Expense_Amount,Create_Date From OwnerExpense Where Business_Name=@business_name And IsDeleted=@is_deleted And cast([Create_Date] as date) between @from_date and @to_date", con);
                    get_oexp_cmd.Parameters.AddWithValue("@from_date", From_Date);
                    get_oexp_cmd.Parameters.AddWithValue("@to_date", To_Date);
                }             
                get_oexp_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                get_oexp_cmd.Parameters.AddWithValue("@is_deleted", '0');
                SqlDataAdapter da = new SqlDataAdapter(get_oexp_cmd);
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
        [Route("api/data/oexp")]
        public IActionResult Create_Owner_Expense([FromBody] OwnerExpenseModel OwnerExpenseModel)
        {
            string connection = Configuration.GetConnectionString(OwnerExpenseModel.DB_Name);
            try
            {
                SqlConnection con = new SqlConnection(connection);
                con.Open();           
                    SqlCommand create_oexp_cmd = new SqlCommand("Insert Into OwnerExpense(Owner_Expense_Detail,Owner_Expense_Amount,Business_Name,Create_Date,IsDeleted)Values(@Owner_expense_detail,@Owner_expense_amount,@business_name,@create_date,@isdeleted)", con);
                    create_oexp_cmd.Parameters.AddWithValue("@Owner_expense_detail", OwnerExpenseModel.Owner_Expense_Detail);
                    create_oexp_cmd.Parameters.AddWithValue("@Owner_expense_amount", OwnerExpenseModel.Owner_Expense_Amount);
                    create_oexp_cmd.Parameters.AddWithValue("@business_name", OwnerExpenseModel.Business_Name);
                    create_oexp_cmd.Parameters.AddWithValue("@create_date", DateTime.Now);
                    create_oexp_cmd.Parameters.AddWithValue("@isdeleted", '0');
                    create_oexp_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status201Created, "Owner Expense Craeted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Create
        #region Update
        [HttpPut]
        [Route("api/data/oexp")]
        public IActionResult Update_City([FromBody] OwnerExpenseModel OwnerExpenseModel)
        {
            try
            {
                string connection = Configuration.GetConnectionString(OwnerExpenseModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();                             
                    SqlCommand update_cmd = new SqlCommand("Update OwnerExpense Set Owner_Expense_Detail=@Owner_expense_detail,Owner_Expense_Amount=@Owner_expense_Amount,Update_Date=@update_date Where OEXP_ID=@oexp_id And Business_Name=@business_name And IsDeleted=@is_deleted", con);
                    update_cmd.Parameters.AddWithValue("@Owner_expense_detail", OwnerExpenseModel.Owner_Expense_Detail);
                    update_cmd.Parameters.AddWithValue("@Owner_expense_amount", OwnerExpenseModel.Owner_Expense_Amount);
                    update_cmd.Parameters.AddWithValue("@oexp_id", OwnerExpenseModel.OEXP_ID);
                    update_cmd.Parameters.AddWithValue("@update_date", DateTime.Now);
                    update_cmd.Parameters.AddWithValue("@business_name", OwnerExpenseModel.Business_Name);
                    update_cmd.Parameters.AddWithValue("@is_deleted", '0');
                    update_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Owner Expense  Updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Update
        #region Delete
        [HttpDelete]
        [Route("api/data/oexp")]
        public IActionResult Delete_City(String OEXP_ID, String Business_Name)
        {
            try
            {
                DefaultModel defaultModel = new DefaultModel();
                string connection = Configuration.GetConnectionString(defaultModel.DB_Name);
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                    SqlCommand delete_cmd = new SqlCommand("Delete From OwnerExpense Where OEXP_ID=@oexp_id And Business_Name=@business_name", con);
                    delete_cmd.Parameters.AddWithValue("@oexp_id", OEXP_ID);
                    delete_cmd.Parameters.AddWithValue("@business_name", Business_Name);
                    delete_cmd.ExecuteNonQuery();
                    con.Close();
                    GC.Collect();
                    return StatusCode(StatusCodes.Status202Accepted, "Owner Expense Deleted.");                              
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
        #endregion Delete

    }
}
